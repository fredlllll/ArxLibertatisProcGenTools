using ArxLibertatisEditorIO.WellDoneIO;
using System.Collections.Generic;

namespace ArxLibertatisProcGenTools.Generators
{
    public interface IMeshGenerator
    {
        IEnumerable<Polygon> GetPolygons();
    }
}
