using ArxLibertatisEditorIO.WellDoneIO;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Modifiers
{
    public interface IModifier
    {
        void Apply(WellDoneArxLevel wdl);
    }
}
