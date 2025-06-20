using System.Numerics;

namespace ArxLibertatisProcGenTools.Values
{
    public interface IValue
    {
        float GetValue(Vector3 input);

        public static readonly IValue NullValue = new NullValue();
    }

    public class NullValue : IValue
    {
        public float GetValue(Vector3 input)
        {
            return 0;
        }
    }
}
