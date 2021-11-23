using ArxLibertatisEditorIO.WellDoneIO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools.MeshGens
{
    public interface IMeshGenerator
    {
        IEnumerable<Polygon> GetPolygons();
    }
}
