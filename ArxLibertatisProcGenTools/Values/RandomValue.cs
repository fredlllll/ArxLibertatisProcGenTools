using System.Numerics;

namespace ArxLibertatisProcGenTools.Values
{
    [Description("random value (uniform)")]
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
