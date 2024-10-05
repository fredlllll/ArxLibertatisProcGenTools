using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Shapes
{
    public class Sphere : IShape
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        public float Radius { get; set; } = 500;
        public float Falloff { get; set; } = 200;
        public Vector3 GetAffectedness(Vector3 position)
        {
            float distance = (position - Center).Length();
            float falloffEnd = Radius + (Falloff / 2);
            if (distance > falloffEnd)
            {
                return Vector3.Zero;
            }
            float falloffStart = Radius - (Falloff / 2);
            if (distance < falloffStart)
            {
                return Vector3.One;
            }
            return (1 - ((distance - falloffStart) / Falloff)) * Vector3.One;
        }

        public Vector3 GetRandomPosition()
        {
            Vector3 randomDirection = new Vector3(
                (float)Util.R.NextDouble() - 0.5f,
                (float)Util.R.NextDouble() - 0.5f,
                (float)Util.R.NextDouble() - 0.5f
                );
            randomDirection = Vector3.Normalize(randomDirection);
            return randomDirection * (float)Util.R.NextDouble();
        }
    }
}
