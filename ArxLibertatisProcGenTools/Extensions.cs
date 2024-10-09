using ArxLibertatisEditorIO.WellDoneIO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public static class Extensions
    {
        public static Vertex Copy(this Vertex v)
        {
            return new Vertex()
            {
                position = v.position,
                uv = v.uv,
                color = v.color,
                normal = v.normal,
            };
        }
    }
}
