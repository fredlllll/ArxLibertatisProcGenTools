using ArxLibertatisEditorIO.WellDoneIO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools.Generators
{
    public interface ILightGenerator
    {
        IEnumerable<ArxLibertatisEditorIO.WellDoneIO.Light> GetLights();
    }
}
