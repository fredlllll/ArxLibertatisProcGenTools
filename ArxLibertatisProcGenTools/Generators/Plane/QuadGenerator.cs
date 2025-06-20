using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using System.Collections.Generic;
using System.Numerics;

namespace ArxLibertatisProcGenTools.Generators.Plane
{
    public class QuadGenerator : IMeshGenerator
    {
        public Vector3 center;
        public float width = 100, height = 100;
        public Vector3 normal = new Vector3(0, -1, 0), worldUp = new Vector3(0, -1, -0.000001f); //offset normal a bit so calculations work hopefully
        public float minU = 0, maxU = 1;
        public float minV = 0, maxV = 1;
        public short room = 1;
        public PolyType polyType;
        public string texturePath = "";
        public float transVal = 0;

        public IEnumerable<Polygon> GetPolygons()
        {
            yield return GetPolygon();
        }

        public Polygon GetPolygon()
        {
            Polygon p = new Polygon();

            Vector3 polyRight = Vector3.Normalize(Vector3.Cross(worldUp, normal)) * width;
            Vector3 polyUp = Vector3.Normalize(Vector3.Cross(normal, polyRight)) * height;

            p.vertices[0].position = center - polyRight / 2 + polyUp / 2;
            p.vertices[0].uv.X = minU;
            p.vertices[0].uv.Y = maxV;
            p.vertices[2].position = center + polyRight / 2 + polyUp / 2;
            p.vertices[2].uv.X = maxU;
            p.vertices[2].uv.Y = maxV;
            p.vertices[1].position = center - polyRight / 2 - polyUp / 2;
            p.vertices[1].uv.X = minU;
            p.vertices[1].uv.Y = minV;
            p.vertices[3].position = center + polyRight / 2 - polyUp / 2;
            p.vertices[3].uv.X = maxU;
            p.vertices[3].uv.Y = minV;

            for (int i = 0; i < 4; ++i)
            {
                p.vertices[i].normal = normal;
                p.vertices[i].color = new Color(1, 1, 1);
            }
            p.norm = p.norm2 = normal;
            p.room = room;
            p.polyType = polyType | PolyType.QUAD;
            p.texturePath = texturePath;
            p.transVal = transVal;
            p.RecalculateArea();

            return p;
        }
    }
}
