using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.Shapes;
using ArxLibertatisProcGenTools.Values;
using SharpNoise.Modules;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Modifiers
{
    public class Rumble : IModifier
    {
        public float Magnitude { get; set; } = 20;

        public IShape Shape { get; set; }

        public IValue NoiseValue { get; set; }

        public void Apply(WellDoneArxLevel wdl)
        {
            foreach (var p in wdl.polygons)
            {
                foreach (var v in p.vertices)
                {
                    var x = NoiseValue.GetValue(v.position);
                    var y = NoiseValue.GetValue(new Vector3(v.position.X, v.position.Y + 10000, v.position.Z));
                    var z = NoiseValue.GetValue(new Vector3(v.position.X, v.position.Y - 10000, v.position.Z));
                    var affectedness = Shape.GetAffectedness(v.position);
                    v.position += new Vector3(x, y, z) * affectedness * Magnitude;
                }
            }
        }
    }
}
