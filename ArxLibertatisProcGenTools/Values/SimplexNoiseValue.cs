using SharpNoise.Modules;
using System.Numerics;

namespace ArxLibertatisProcGenTools.Values
{
    [Description("random value (simplex, like perlin but better), get Noise property to change parameters")]
    public class SimplexNoiseValue : IValue
    {
        public Simplex Noise { get; } = new Simplex();

        public float GetValue(Vector3 input)
        {
            return (float)Noise.GetValue(input.X, input.Y, input.Z);
        }
    }
}
