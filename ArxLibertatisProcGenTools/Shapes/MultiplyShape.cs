using System.Numerics;

namespace ArxLibertatisProcGenTools.Shapes
{
    [Description("returns the multiplication of two shapes")]
    public class MultiplyShape : IShape
    {
        public IShape Shape1 { get; set; } = IShape.NullShape;
        public IShape Shape2 { get; set; } = IShape.NullShape;

        public Vector3 GetAffectedness(Vector3 position)
        {
            Vector3 a = Shape1.GetAffectedness(position);
            Vector3 b = Shape2.GetAffectedness(position);
            return a * b;
        }

        public Vector3 GetRandomPosition()
        {
            return Shape1.GetRandomPosition() * Shape2.GetRandomPosition();
        }
    }
}
