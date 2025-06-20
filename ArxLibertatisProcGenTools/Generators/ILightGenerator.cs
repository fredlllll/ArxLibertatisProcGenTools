using System.Collections.Generic;

namespace ArxLibertatisProcGenTools.Generators
{
    public interface ILightGenerator
    {
        IEnumerable<ArxLibertatisEditorIO.WellDoneIO.Light> GetLights();
    }
}
