using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools.Generators
{
    public interface ITextureGenerator
    {
        string GetTexturePath(int polygonIndex);
    }
}
