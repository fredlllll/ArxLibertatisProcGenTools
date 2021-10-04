using ArxLibertatisEditorIO;
using ArxLibertatisEditorIO.IO.FTS;
using ArxLibertatisEditorIO.IO.Shared_IO;
using ArxLibertatisEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public class ArxLevel
    {
        public readonly List<Primitive> primitives = new List<Primitive>();

        public void LoadFrom(ArxLevelRaw level)
        {
            int lightIndex = 0;
            var fts = level.FTS;
            var llf = level.LLF;

            Dictionary<int, int> tcToIndex = new Dictionary<int, int>();
            for (int i = 0; i < fts.textureContainers.Length; i++)
            {
                tcToIndex[fts.textureContainers[i].tc] = i;
            }

            for (int c = 0; c < fts.cells.Length; c++)
            {
                var cell = fts.cells[c];
                for (int p = 0; p < cell.polygons.Length; p++)
                {
                    var poly = cell.polygons[p];

                    string texArxPath = "";
                    if (tcToIndex.TryGetValue(poly.tex, out int textureIndex))
                    {
                        texArxPath = IOHelper.GetString(fts.textureContainers[textureIndex].fic);
                    }
                    else
                    {
                        Logging.LogInfo("Couldnt find texture " + poly.tex);
                    }


                    Primitive primitive = new Primitive
                    {
                        polyType = poly.type,
                        norm = poly.norm.ToVector3(),
                        norm2 = poly.norm2.ToVector3(),
                        area = poly.area,
                        room = poly.room,
                        paddy = poly.paddy,
                        texturePath = texArxPath,
                        transVal = poly.transval
                    };

                    int vertCount = primitive.VertexCount;
                    for (int i = 0; i < vertCount; i++)
                    {
                        var vert = poly.vertices[i];
                        primitive.vertices[i] = new Vertex(new Vector3(vert.posX, vert.posY, vert.posZ),
                            new Vector2(vert.texU, 1 - vert.texV),
                            poly.normals[i].ToVector3(),
                            IOHelper.FromBGRA(llf.lightColors[lightIndex++]));
                    }

                    if (primitive.IsTriangle)
                    {
                        //load 4th vertex manually as it has no lighting value and would break lighting otherwise
                        var lastVert = poly.vertices[3];
                        primitive.vertices[3] = new Vertex(new Vector3(lastVert.posX, lastVert.posY, lastVert.posZ),
                            new Vector2(lastVert.texU, 1 - lastVert.texV),
                            poly.normals[3].ToVector3(),
                            new Color(1, 1, 1));
                    }

                    primitives.Add(primitive);
                }
            }
        }

        class LevelCell
        {
            public readonly int x, z;
            public readonly List<Primitive> primitives = new List<Primitive>();

            public LevelCell(int x, int z)
            {
                this.x = x;
                this.z = z;
            }

            public void AddPrimitive(Primitive primitive)
            {
                primitives.Add(primitive);
            }
        }

        static Vector2Int GetPrimitiveCellPos(Primitive prim)
        {
            Vector2 pos = Vector2.Zero;

            int vertCount = prim.VertexCount;
            for (int i = 0; i < vertCount; i++)
            {
                var v = prim.vertices[i];
                pos += new Vector2(v.position.X, v.position.Z);
            }
            pos /= vertCount;
            return new Vector2Int((int)(pos.X / 100), (int)(pos.Y / 100));
        }

        public void WriteTo(ArxLevelRaw level)
        {
            // create texture containers

            HashSet<string> uniqueTexturePaths = new HashSet<string>();

            var fts = level.FTS;

            //create cells
            int sizex = fts.sceneHeader.sizex;
            int sizez = fts.sceneHeader.sizez;
            LevelCell[] cells = new LevelCell[sizex * sizez];
            for (int z = 0, index = 0; z < sizez; z++)
            {
                for (int x = 0; x < sizex; x++, index++)
                {
                    var cell = new LevelCell(x, z);
                    cells[index] = cell;
                }
            }

            //add primitives to cells
            Vector2Int maxPos = new Vector2Int(sizex - 1, sizez - 1); //if clamp is inclusive we have to sub 1
            foreach (var prim in primitives)
            {
                var cellpos = GetPrimitiveCellPos(prim);
                cellpos.Clamp(Vector2Int.Zero, maxPos);
                var cell = cells[IOHelper.XZToCellIndex(cellpos.x, cellpos.y, sizex, sizez)];
                cell.AddPrimitive(prim);
                uniqueTexturePaths.Add(prim.texturePath);
            }

            fts.textureContainers = new FTS_IO_TEXTURE_CONTAINER[uniqueTexturePaths.Count];
            int tci = 0; //nothing speaks against just using a normal index for tc, i dont know why they ever used random ints
            Dictionary<string, int> texPathToTc = new Dictionary<string, int>();
            foreach (var path in uniqueTexturePaths)
            {
                int index = tci++;
                fts.textureContainers[index].fic = IOHelper.GetBytes(path, 256);
                texPathToTc[path] = fts.textureContainers[index].tc = index;
            }

            List<FTS_IO_EP_DATA>[] roomPolyDatas = new List<FTS_IO_EP_DATA>[fts.rooms.Length];
            for (int i = 0; i < fts.rooms.Length; ++i)
            {
                roomPolyDatas[i] = new List<FTS_IO_EP_DATA>();
            }

            List<uint> lightColors = new List<uint>();

            //put primitves into polys in their cells
            for (int z = 0, index = 0; z < sizez; z++)
            {
                for (int x = 0; x < sizex; x++, index++)
                {
                    //int index = ArxIOHelper.XZToCellIndex(x, z, sizex, sizez);
                    var myCell = cells[index];
                    var ftsCell = fts.cells[index];
                    ftsCell.sceneInfo.nbpoly = myCell.primitives.Count;
                    ftsCell.polygons = new FTS_IO_EERIEPOLY[myCell.primitives.Count];
                    for (int i = 0; i < myCell.primitives.Count; i++)
                    {
                        var prim = myCell.primitives[i];
                        var poly = new FTS_IO_EERIEPOLY();
                        //copy data over
                        poly.area = prim.area;
                        poly.norm = new SavedVec3(prim.norm);
                        poly.norm2 = new SavedVec3(prim.norm2);
                        poly.paddy = prim.paddy;
                        poly.room = prim.room;
                        poly.type = prim.polyType;
                        poly.tex = texPathToTc[prim.texturePath];
                        poly.transval = prim.transVal;

                        if (poly.room >= 0)
                        {
                            var polyData = new FTS_IO_EP_DATA();
                            polyData.cell_x = (short)x;
                            polyData.cell_z = (short)z;
                            polyData.idx = (short)i;
                            roomPolyDatas[poly.room].Add(polyData);
                        }

                        //copy vertices
                        poly.vertices = new FTS_IO_VERTEX[4];
                        poly.normals = new SavedVec3[4];
                        for (int j = 0; j < 4; j++) //always save all 4 vertices regardless of if its a triangle or quad
                        {
                            var vert = prim.vertices[j];
                            var natVert = new FTS_IO_VERTEX();
                            natVert.posX = vert.position.X;
                            natVert.posY = vert.position.Y;
                            natVert.posZ = vert.position.Z;

                            natVert.texU = vert.uv.X;
                            natVert.texV = 1 - vert.uv.Y;
                            poly.normals[j] = new SavedVec3(vert.normal);

                            poly.vertices[j] = natVert;
                        }

                        lightColors.Add(IOHelper.ToBGRA(prim.vertices[0].color));
                        lightColors.Add(IOHelper.ToBGRA(prim.vertices[1].color));
                        lightColors.Add(IOHelper.ToBGRA(prim.vertices[2].color));
                        if (poly.type.HasFlag(PolyType.QUAD))
                        {
                            lightColors.Add(IOHelper.ToBGRA(prim.vertices[3].color));
                        }

                        ftsCell.polygons[i] = poly;
                    }
                    fts.cells[index] = ftsCell;
                }
            }

            for (int i = 0; i < fts.rooms.Length; i++)
            {
                fts.rooms[i].polygons = roomPolyDatas[i].ToArray();
            }

            //update llf
            var llf = level.LLF;
            llf.lightingHeader.numLights = lightColors.Count;
            llf.lightColors = lightColors.ToArray();
            //below does the same as toArray, just wondering if toArray is faster or manually assigning it
            /*llf.lightColors = new uint[lightColors.Count];
            for (int i = 0; i < lightColors.Count; i++)
            {
                llf.lightColors[i] = lightColors[i];
            }*/
        }
    }
}
