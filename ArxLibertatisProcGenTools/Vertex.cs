using ArxLibertatisEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public class Vertex
    {
        public Vector3 position;
        public Vector2 uv;
        public Vector3 normal;
        public Color color;

        public Vertex(Vector3 pos, Vector2 uv, Vector3 norm, Color color)
        {
            this.position = pos;
            this.uv = uv;
            this.normal = norm;
            this.color = color;
        }
    }
}
