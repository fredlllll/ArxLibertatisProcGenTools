using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using System.Collections.Generic;
using System.Numerics;

namespace ArxLibertatisProcGenTools.Generators.Plane
{
    [Description("Generates a single quad in a certain orientation")]
    public class QuadGenerator : IMeshGenerator
    {
        public Vector3 Center { get; set; }
        public float Width { get; set; } = 100;
        public float Height { get; set; } = 100;
        public Vector3 Normal { get; set; } = new Vector3(0, -1, 0);
        public Vector3 WorldUp { get; set; } = new Vector3(0, -1, -0.000001f); //offset normal a bit so calculations work hopefully

        public float MinU { get; set; } = 0;
        public float MaxU { get; set; } = 1;
        public float MinV { get; set; } = 0;
        public float MaxV { get; set; } = 1;
        public short Room { get; set; } = 1;
        public PolyType PolyType { get; set; }
        public string TexturePath { get; set; } = "";
        public float TransVal { get; set; } = 0;

        public IEnumerable<Polygon> GetPolygons()
        {
            yield return GetPolygon();
        }

        public Polygon GetPolygon()
        {
            Polygon p = new Polygon();

            Vector3 polyRight = Vector3.Normalize(Vector3.Cross(WorldUp, Normal)) * Width;
            Vector3 polyUp = Vector3.Normalize(Vector3.Cross(Normal, polyRight)) * Height;

            p.vertices[0].position = Center - polyRight / 2 + polyUp / 2;
            p.vertices[0].uv.X = MinU;
            p.vertices[0].uv.Y = MaxV;
            p.vertices[2].position = Center + polyRight / 2 + polyUp / 2;
            p.vertices[2].uv.X = MaxU;
            p.vertices[2].uv.Y = MaxV;
            p.vertices[1].position = Center - polyRight / 2 - polyUp / 2;
            p.vertices[1].uv.X = MinU;
            p.vertices[1].uv.Y = MinV;
            p.vertices[3].position = Center + polyRight / 2 - polyUp / 2;
            p.vertices[3].uv.X = MaxU;
            p.vertices[3].uv.Y = MinV;

            for (int i = 0; i < 4; ++i)
            {
                p.vertices[i].normal = Normal;
                p.vertices[i].color = new Color(1, 1, 1);
            }
            p.norm = p.norm2 = Normal;
            p.room = Room;
            p.polyType = PolyType | PolyType.QUAD;
            p.texturePath = TexturePath;
            p.transVal = TransVal;
            p.RecalculateArea();

            return p;
        }
    }
}
