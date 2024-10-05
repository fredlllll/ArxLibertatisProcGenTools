using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.Generators;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Generators.Plane
{
    public class FloorGenerator : IMeshGenerator
    {
        private Vector3 min = Vector3.Zero, max = Vector3.UnitX + Vector3.UnitZ;

        public ITextureGenerator TextureGenerator { get; set; }

        public Vector2 Size
        {
            get
            {
                var tmp = max - min;
                return new Vector2(tmp.X, tmp.Z);
            }
            set
            {
                var center = Center;
                min = new Vector3(center.X - (value.X / 2), center.Y, center.Z - (value.Y / 2));
                max = new Vector3(center.X + (value.X / 2), center.Y, center.Z + (value.Y / 2));
            }
        }

        public Vector3 Center
        {
            get
            {
                return (max + min) / 2;
            }
            set
            {
                var currentCenter = Center;
                var difference = value - currentCenter;
                min += difference;
                max += difference;
            }
        }

        public IEnumerable<Polygon> GetPolygons()
        {
            float xStart = min.X;
            float zStart = min.Z;

            var size = Size;
            var center = Center;

            int segmentsX = (int)MathF.Ceiling(size.X / 100);
            int segmentsZ = (int)MathF.Ceiling(size.X / 100);
            float xStep = size.X / segmentsX;
            float zStep = size.Y / segmentsZ;

            QuadGenerator quadGen = new QuadGenerator();
            quadGen.width = xStep;
            quadGen.height = zStep;

            int polygonIndex = 0;
            Vector3 here = new Vector3(xStart-xStep/2,center.Y,zStart-zStep/2);
            for (int x = 0; x < segmentsX; ++x)
            {
                here.X += xStep;
                for (int z = 0; z < segmentsZ; ++z, ++polygonIndex)
                {
                    here.Z += zStep;
                    if (TextureGenerator != null) {
                        quadGen.texturePath = TextureGenerator.GetTexturePath(polygonIndex);
                    }

                    quadGen.center = here;

                    yield return quadGen.GetPolygon();
                }
                here.Z = zStart - zStep / 2;
            }
        }
    }
}
