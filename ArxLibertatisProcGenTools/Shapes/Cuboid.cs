using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Shapes
{
    public class Cuboid : IShape
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        public Vector3 GetAffectedness(Vector3 position)
        {
            var biggerThanMin =
                position.X > Min.X &&
                position.Y > Min.Y &&
                position.Z > Min.Z;
            var smallerThanMax = position.X < Max.X &&
                position.Y < Max.Y &&
                position.Z < Max.Z;
            if (biggerThanMin && smallerThanMax)
            {
                return Vector3.One;
            }
            return Vector3.Zero;
        }

        public Vector3 GetRandomPosition()
        {
            var difference = Max - Min;
            return Min + new Vector3(
                (float)Util.R.NextDouble() * difference.X,
                (float)Util.R.NextDouble() * difference.Y,
                (float)Util.R.NextDouble() * difference.Z
                );
        }
    }
}
