using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Values
{
    public interface IValue
    {
        float GetValue(Vector3 input);
    }
}
