using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Generators.Mesh
{
    /// <summary>
    /// only supports triangulated obj files
    /// </summary>
    public class OBJImporter : IMeshGenerator
    {
        public Matrix4x4 worldMatrix = Matrix4x4.Identity;
        public short room = 1;

        private class Vertex
        {
            public int v = -1, uv = -1, n = -1;
        }
        private class Face
        {
            public string texture = null;

            public Vertex[] vertices = new Vertex[] { new Vertex(), new Vertex(), new Vertex() };
        }

        private class Material
        {
            public string texture;
        }

        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector2> textureCoordinates = new List<Vector2>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Face> faces = new List<Face>();
        private Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public OBJImporter(string objFilePath, string mtlFilePath)
        {
            ParseMtl(mtlFilePath);
            ParseObj(objFilePath);
        }

        private float ParseFloat(string s)
        {
            return float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }

        private int ParseInt(string s)
        {
            return int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }

        private void ParseMtl(string mtlFilePath)
        {
            string matName = null;

            using (var fs = new FileStream(mtlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var s = new StreamReader(fs))
            {
                while (!s.EndOfStream)
                {
                    var l = s.ReadLine().Trim();
                    var lineParts = l.Split(' ');
                    switch (lineParts[0])
                    {
                        case "newmtl":
                            matName = lineParts[1];
                            break;
                        case "map_Kd":
                            materials[matName] = new Material() { texture = l.Substring(lineParts[0].Length + 1).Trim() };
                            break;
                    }
                }
            }
        }

        private void ParseObj(string objFilePath)
        {
            string currentTexture = null;

            using (var fs = new FileStream(objFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var s = new StreamReader(fs))
            {
                while (!s.EndOfStream)
                {
                    var l = s.ReadLine().Trim();
                    var lineParts = l.Split(' ');
                    switch (lineParts[0])
                    {
                        case "v":
                            vertices.Add(new Vector3(ParseFloat(lineParts[1]), ParseFloat(lineParts[2]), ParseFloat(lineParts[3])));
                            break;
                        case "vt":
                            textureCoordinates.Add(new Vector2(ParseFloat(lineParts[1]), ParseFloat(lineParts[2])));
                            break;
                        case "vn":
                            normals.Add(new Vector3(ParseFloat(lineParts[1]), ParseFloat(lineParts[2]), ParseFloat(lineParts[3])));
                            break;
                        case "usemtl":
                            currentTexture = materials[l.Substring(lineParts[0].Length + 1).Trim()].texture;
                            break;
                        case "f":
                            var f = new Face();
                            ParseVertex(lineParts[1], f, 0);
                            ParseVertex(lineParts[2], f, 2);
                            ParseVertex(lineParts[3], f, 1);
                            f.texture = currentTexture;
                            faces.Add(f);
                            break;
                    }
                }
            }

        }

        private void ParseVertex(string v, Face f, int index)
        {
            var parts = v.Split('/');
            f.vertices[index].v = ParseInt(parts[0]) - 1;
            if (parts[1].Length > 0)
            {
                f.vertices[index].uv = ParseInt(parts[1]) - 1;
            }
            if (parts[2].Length > 0)
            {
                f.vertices[index].n = ParseInt(parts[2]) - 1;
            }
        }

        public IEnumerable<Polygon> GetPolygons()
        {
            for (int i = 0; i < faces.Count; ++i)
            {
                var f = faces[i];
                var p = new Polygon();

                p.room = room;
                p.texturePath = f.texture;

                for (int j = 0; j < 3; ++j)
                {
                    p.vertices[j].position = Vector3.Transform(vertices[f.vertices[j].v], worldMatrix);
                    p.vertices[j].position.Y = -p.vertices[j].position.Y; //invert Y cause thats how arx rolls
                    p.vertices[j].uv = textureCoordinates[f.vertices[j].uv];
                    p.vertices[j].uv.Y = 1 - p.vertices[j].uv.Y; //arx again
                    p.vertices[j].normal = Vector3.TransformNormal(normals[f.vertices[j].n], worldMatrix);
                    p.vertices[j].normal.Y = -p.vertices[j].normal.Y; //fingers crossed it works for normals too
                    p.vertices[j].color = new Color(1, 1, 1);
                }

                p.RecalculateNormals();
                p.RecalculateArea();

                yield return p;
            }
        }
    }
}
