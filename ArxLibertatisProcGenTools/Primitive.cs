using ArxLibertatisEditorIO.Util;
using System.Numerics;

namespace ArxLibertatisProcGenTools
{
    public class Primitive
    {
        public readonly Vertex[] vertices = new Vertex[4];

        public Vector3 norm;
        public Vector3 norm2;
        public float area;
        public short room;
        public short paddy;
        public PolyType polyType;
        public string texturePath;
        public float transVal;

        public int VertexCount
        {
            get { return polyType.HasFlag(PolyType.QUAD) ? 4 : 3; }
        }

        public bool IsQuad
        {
            get { return polyType.HasFlag(PolyType.QUAD); }
        }

        public bool IsTriangle
        {
            get { return !IsQuad; }
        }
    }
}
