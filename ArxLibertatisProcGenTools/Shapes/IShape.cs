using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Shapes
{
    public interface IShape
    {
        Vector3 GetAffectedness(Vector3 position);

        Vector3 GetRandomPosition();
    }

    public static class IShapeExtensions
    {
        public static IShape MultiplyWith(this IShape s1, IShape s2)
        {
            return new MultiplyShape() { Shape1 = s1, Shape2 = s2 };
        }
    }
}
