using ArxLibertatisEditorIO.Util;
using ArxLibertatisProcGenTools.Shapes;
using ArxLibertatisProcGenTools.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools.Generators.Light
{
    public class RandomLightGenerator : ILightGenerator
    {
        public int Count { get; set; } = 1;

        public IShape PositionShape { get; set; }
        public IShape ColorShape { get; set; }
        public IValue FalloffStart { get; set; }
        public IValue FalloffEnd { get; set; }
        public IValue Intensity { get; set; }

        public IEnumerable<ArxLibertatisEditorIO.WellDoneIO.Light> GetLights()
        {
            for (int i = 0; i < Count; i++)
            {
                var light = new ArxLibertatisEditorIO.WellDoneIO.Light();
                light.position = PositionShape.GetRandomPosition();
                var tmp = ColorShape.GetRandomPosition();
                light.color = new Color(tmp.X, tmp.Y, tmp.Z);
                light.fallStart = FalloffStart.GetValue(light.position);
                light.fallEnd = FalloffEnd.GetValue(light.position);
                light.intensity = Intensity.GetValue(light.position);
                yield return light;
            }
        }
    }
}
