using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Values
{
    public class FixedValue : IValue
    {
        public FixedValue() { }
        public FixedValue(float value)
        {
            Value = value;
        }
        public FixedValue(double value)
        {
            Value = (float)value;
        }

        public float Value { get; set; }
        public float GetValue(Vector3 input)
        {
            return Value;
        }
    }
}
