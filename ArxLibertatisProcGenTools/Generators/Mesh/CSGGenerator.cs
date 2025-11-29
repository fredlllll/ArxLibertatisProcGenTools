using ArxLibertatisEditorIO.Util;
using Csg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using static Csg.Solids;

namespace ArxLibertatisProcGenTools.Generators.Mesh
{
    [Description("Adds CSG solids to the level")]
    public class CSGGenerator : IMeshGenerator
    {
        public Solid CsgSolid { get; set; } = Cube(1, true);
        public Vector3 PositionOffset { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
        public Vector3 UVScale { get; set; } = Vector3.One;
        public PolyType PolyType { get; set; } = PolyType.None;
        public short Room { get; set; } = 1;
        public ITextureGenerator? TextureGenerator { get; set; }

        public IEnumerable<ArxLibertatisEditorIO.WellDoneIO.Polygon> GetPolygons()
        {
            if (TextureGenerator == null)
            {
                throw new InvalidOperationException("No texture generator set on csg generator");
            }
            int i = 0;
            foreach (var triangle in GetTriangles(CsgSolid, UVScale))
            {
                var arxPoly = new ArxLibertatisEditorIO.WellDoneIO.Polygon();
                arxPoly.room = Room;
                var normal = triangle.polygon.Plane.Normal.ToVector3();

                VertexConversion(triangle.a, ref arxPoly.vertices[0], PositionOffset, Scale, normal);
                VertexConversion(triangle.b, ref arxPoly.vertices[1], PositionOffset, Scale, normal);
                VertexConversion(triangle.c, ref arxPoly.vertices[2], PositionOffset, Scale, normal);
                arxPoly.polyType = PolyType & ~PolyType.QUAD;
                arxPoly.texturePath = TextureGenerator.GetTexturePath(i++);
                yield return arxPoly;
            }
        }

        public BoundingBox GetSolidBounds()
        {
            //Bounds is private for some reason, reflection it is
            var boundsProp = typeof(Solid).GetProperty("Bounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var bounds = (BoundingBox)boundsProp.GetValue(CsgSolid, null);
            return bounds;
        }

        public Vector3 GetSolidSize()
        {
            var bounds = GetSolidBounds();
            var min = bounds.Min;
            var max = bounds.Max;

            var sizeX = max.X - min.X;
            var sizeY = max.Y - min.Y;
            var sizeZ = max.Z - min.Z;
            return new Vector3((float)sizeX, (float)sizeY, (float)sizeZ);
        }

        public Vector3 GetFinalSize()
        {
            var bounds = GetSolidBounds();
            var min = bounds.Min;
            var max = bounds.Max;

            var sizeX = max.X - min.X;
            var sizeY = max.Y - min.Y;
            var sizeZ = max.Z - min.Z;
            return new Vector3((float)sizeX * Scale.X, (float)sizeY * Scale.Y, (float)sizeZ * Scale.Z);
        }

        private struct Triangle
        {
            public Polygon polygon;
            public Vertex a, b, c;
        }

        private enum Axis
        {
            X, Y, Z
        }

        private static IEnumerable<Triangle> GetTriangles(Solid solid, Vector3 UVscale)
        {
            //Bounds is private for some reason, reflection it is
            var boundsProp = typeof(Solid).GetProperty("Bounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var bounds = (BoundingBox)boundsProp.GetValue(solid, null);
            var min = bounds.Min;
            var max = bounds.Max;

            var sizeX = max.X - min.X;
            var sizeY = max.Y - min.Y;
            var sizeZ = max.Z - min.Z;

            //apply texture coordinates first
            var polys = solid.Polygons;
            foreach (var poly in polys)
            {
                var n = poly.Plane.Normal;
                Axis majorAxis = Axis.Z;
                if (Math.Abs(n.X) >= Math.Abs(n.Y) && Math.Abs(n.X) >= Math.Abs(n.Z))
                    majorAxis = Axis.X;
                else if (Math.Abs(n.Y) >= Math.Abs(n.Z))
                    majorAxis = Axis.Y;

                for (int i = 0; i < poly.Vertices.Count; ++i)
                {
                    var p = poly.Vertices[i].Pos;
                    double u, v;
                    switch (majorAxis)
                    {
                        case Axis.X:
                            u = (p.Y - min.Y) / sizeY * UVscale.Y;
                            v = (p.Z - min.Z) / sizeZ * UVscale.Z;
                            break;
                        case Axis.Y:
                            u = (p.X - min.X) / sizeX * UVscale.X;
                            v = (p.Z - min.Z) / sizeZ * UVscale.Z;
                            break;
                        case Axis.Z:
                            u = (p.X - min.X) / sizeX * UVscale.X;
                            v = (p.Y - min.Y) / sizeY * UVscale.Y;
                            break;
                        default:
                            throw new InvalidDataException();
                    }
                    //cant just set the tex vector cause its readonly, have to replace the vertex
                    poly.Vertices[i] = new Vertex(p, new Vector2D(u, v));
                }
            }

            //apparently its always triangle fans
            for (int i = 0; i < polys.Count; i++)
            {
                var polygon = polys[i];
                for (int j = 2; j < polygon.Vertices.Count; j++)
                {
                    yield return new Triangle()
                    {
                        polygon = polygon,
                        a = polygon.Vertices[0],
                        b = polygon.Vertices[j - 1],
                        c = polygon.Vertices[j]
                    };
                }
            }
        }

        private static void VertexConversion(Vertex v, ref ArxLibertatisEditorIO.WellDoneIO.Vertex target, Vector3 positionOffset, Vector3 scale, Vector3 normal)
        {
            var pos = new Vector3((float)v.Pos.X, (float)v.Pos.Y, (float)v.Pos.Z);
            Vector3D scale_ = new Vector3D(scale.X, scale.Y, scale.Z);
            Vector3D positionOffset_ = new Vector3D(positionOffset.X, positionOffset.Y, positionOffset.Z);
            target.position = ((v.Pos * scale_) + positionOffset_).ToVector3();
            target.normal = normal;
            target.uv = v.Tex.ToVector2();
        }
    }

    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Vector3D v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Vector2 ToVector2(this Vector2D v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }
    }
}
