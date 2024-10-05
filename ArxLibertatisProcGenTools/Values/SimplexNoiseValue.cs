using SharpNoise.Modules;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Values
{
    public class SimplexNoiseValue : IValue
    {
        public Simplex Noise { get; } = new Simplex();

        public float GetValue(Vector3 input)
        {
            return (float)Noise.GetValue(input.X, input.Y, input.Z);
        }
    }
}
