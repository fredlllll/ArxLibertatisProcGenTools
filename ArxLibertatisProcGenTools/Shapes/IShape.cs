using System.Numerics;

namespace ArxLibertatisProcGenTools.Shapes
{
    public interface IShape
    {
        Vector3 GetAffectedness(Vector3 position);

        Vector3 GetRandomPosition();

        public static readonly IShape NullShape = new NullShape();
    }

    [Description("Always returns zero")]
    public class NullShape : IShape
    {
        public Vector3 GetAffectedness(Vector3 position)
        {
            return Vector3.Zero;
        }

        public Vector3 GetRandomPosition()
        {
            return Vector3.Zero;
        }
    }

    public static class IShapeExtensions
    {
        public static IShape MultiplyWith(this IShape s1, IShape s2)
        {
            return new MultiplyShape() { Shape1 = s1, Shape2 = s2 };
        }
    }
}
