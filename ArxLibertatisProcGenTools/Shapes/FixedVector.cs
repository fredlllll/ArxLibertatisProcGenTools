using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Shapes
{
    public class FixedVector : IShape
    {
        public FixedVector() { }
        public FixedVector(Vector3 value)
        {
            Value = value;
        }

        public Vector3 Value { get; set; }
        public Vector3 GetAffectedness(Vector3 position)
        {
            return Value;
        }

        public Vector3 GetRandomPosition()
        {
            return Value;
        }
    }
}
