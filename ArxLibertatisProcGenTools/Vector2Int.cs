using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public struct Vector2Int
    {
        public int x, y;
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Clamp(Vector2Int min, Vector2Int max)
        {
            if (min.x > x)
            {
                x = min.x;
            }
            else if (max.x < x)
            {
                x = max.x;
            }
            if (min.y > y)
            {
                y = min.y;
            }
            else if (max.y < y)
            {
                y = max.y;
            }
        }

        public static Vector2Int Zero
        {
            get { return new Vector2Int(0, 0); }
        }
    }
}
