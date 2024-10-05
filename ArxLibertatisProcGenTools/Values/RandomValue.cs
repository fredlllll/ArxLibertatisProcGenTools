using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Values
{
    public class RandomValue : IValue
    {
        public float Min { get; set; } = 0;
        public float Max { get; set; } = 1;

        public float GetValue(Vector3 input)
        {
            var diff = Max - Min;
            return Min + ((float)Util.R.NextDouble() * diff);
        }
    }
}
