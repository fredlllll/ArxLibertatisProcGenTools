using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Shapes
{
    public class MultiplyShape : IShape
    {
        public IShape Shape1 { get; set; }
        public IShape Shape2 { get; set; }

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
