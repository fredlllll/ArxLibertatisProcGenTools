using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Shapes
{
    public class Everywhere : IShape
    {
        public Vector3 GetAffectedness(Vector3 position)
        {
            return Vector3.One;
        }

        public Vector3 GetRandomPosition()
        {
            return new Vector3((float)Util.R.NextDouble() * 16000, (float)Util.R.NextDouble() * -10000, (float)Util.R.NextDouble() * 16000);
        }
    }
}
