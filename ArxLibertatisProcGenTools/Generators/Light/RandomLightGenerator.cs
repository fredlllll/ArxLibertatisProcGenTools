using ArxLibertatisEditorIO.Util;
using ArxLibertatisProcGenTools.Shapes;
using ArxLibertatisProcGenTools.Values;
using System.Collections.Generic;

namespace ArxLibertatisProcGenTools.Generators.Light
{
    public class RandomLightGenerator : ILightGenerator
    {
        public int Count { get; set; } = 1;

        public IShape PositionShape { get; set; } = IShape.NullShape;
        public IShape ColorShape { get; set; } = IShape.NullShape;
        public IValue FalloffStart { get; set; } = IValue.NullValue;
        public IValue FalloffEnd { get; set; } = IValue.NullValue;
        public IValue Intensity { get; set; } = IValue.NullValue;

        public IEnumerable<ArxLibertatisEditorIO.WellDoneIO.Light> GetLights()
        {
            for (int i = 0; i < Count; i++)
            {
                var tmp = ColorShape.GetRandomPosition();
                var pos = PositionShape.GetRandomPosition();
                var light = new ArxLibertatisEditorIO.WellDoneIO.Light
                {
                    position = pos,
                    color = new Color(tmp.X, tmp.Y, tmp.Z),
                    fallStart = FalloffStart.GetValue(pos),
                    fallEnd = FalloffEnd.GetValue(pos),
                    intensity = Intensity.GetValue(pos)
                };
                yield return light;
            }
        }
    }
}
