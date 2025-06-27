using ArxLibertatisEditorIO.MediumIO.DLF;
using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using CSG;
using CSG.Shapes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ArxLibertatisProcGenTools.Generators.Mesh
{
    [Description("Adds CSG shapes to the level")]
    public class CSGGenerator : IMeshGenerator
    {
        public Shape CsgShape { get; set; } = new Cube(Vector3.Zero, Vector3.One);
        public Vector3 PositionOffset { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
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
            foreach (var triangle in GetTriangles(CsgShape))
            {
                var arxPoly = new ArxLibertatisEditorIO.WellDoneIO.Polygon();
                arxPoly.room = Room;

                VertexConversion(triangle.a, ref arxPoly.vertices[0], PositionOffset, Scale);
                VertexConversion(triangle.b, ref arxPoly.vertices[1], PositionOffset, Scale);
                VertexConversion(triangle.c, ref arxPoly.vertices[2], PositionOffset, Scale);
                arxPoly.polyType = PolyType & ~PolyType.QUAD;
                arxPoly.texturePath = TextureGenerator.GetTexturePath(i++);
                yield return arxPoly;
            }
        }

        public static void CSGTest()
        {
            var cube = new Cube(new Vector3(6000, 500, 6000), new Vector3(140, 140, 140));
            var sphere = new Sphere(new Vector3(6000, 500, 6000), radius: 100);
            var cyl1 = new Cylinder(new Vector3(6000, 0, 6000), new Vector3(6000, 1000, 6000));
            var cyl2 = new Cylinder(new Vector3(5000, 500, 6000), new Vector3(7000, 500, 6000));
            var cyl3 = new Cylinder(new Vector3(6000, 500, 5000), new Vector3(6000, 500, 7000));
            var csgShape = cube.Intersect(sphere).Subtract(cyl1.Union(cyl2).Union(cyl3));
        }

        private struct Triangle
        {
            public CSG.Vertex a, b, c;
        }

        private static IEnumerable<Triangle> GetTriangles(Shape shape)
        {
            //this is a primitive triangulation method i grabbed from the library itself, so i can only hope it will always work.
            //it creates a triangle fan starting from the first vertex
            var polys = shape.CreatePolygons();
            for (int i = 0; i < polys.Length; i++)
            {
                CSG.Polygon polygon = polys[i];
                for (int j = 2; j < polygon.Vertices.Count; j++)
                {
                    yield return new Triangle()
                    {
                        a = polygon.Vertices[0],
                        b = polygon.Vertices[j - 1],
                        c = polygon.Vertices[j]
                    };
                }
            }
        }

        private static void VertexConversion(CSG.Vertex v, ref ArxLibertatisEditorIO.WellDoneIO.Vertex target, Vector3 positionOffset, Vector3 scale)
        {
            target.position = (v.Position * scale) + positionOffset;
            target.normal = v.Normal;
            target.uv = v.TexCoords;
            target.color = new Color(v.Color.X, v.Color.Y, v.Color.Z);
        }
    }
}
