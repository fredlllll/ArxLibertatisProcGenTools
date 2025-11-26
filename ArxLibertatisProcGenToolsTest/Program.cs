using ArxLibertatisEditorIO;
using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.RawIO;
using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools;
using ArxLibertatisProcGenTools.Generators.Light;
using ArxLibertatisProcGenTools.Generators.Mesh;
using ArxLibertatisProcGenTools.Generators.Plane;
using ArxLibertatisProcGenTools.Generators.Texture;
using ArxLibertatisProcGenTools.Modifiers;
using ArxLibertatisProcGenTools.Shapes;
using ArxLibertatisProcGenTools.Values;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace ArxLibertatisProcGenToolsTest
{
    class Program
    {
        static void Monke()
        {
            var ral = new RawArxLevel();
            ral.LoadLevel("level1");
            var mal = new MediumArxLevel();
            mal.LoadFrom(ral);
            var wdl = new WellDoneArxLevel();
            wdl.LoadFrom(mal);


            //monkepos 8550 3050 8800

            OBJImporter o = new OBJImporter(@"C:\Users\Freddy\Desktop\monke.obj", @"C:\Users\Freddy\Desktop\monke.mtl");
            o.Room = 40;

            Vector3 pos = new Vector3(8550, -3050, 8800);

            var translation = Matrix4x4.CreateTranslation(pos);
            var scale = Matrix4x4.CreateScale(100);
            o.WorldMatrix = scale * translation;

            wdl.polygons.AddRange(o.GetPolygons());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            wdl.SaveTo(mal);
            Console.WriteLine("Well done :" + sw.Elapsed);
            sw.Restart();
            mal.SaveTo(ral);
            Console.WriteLine("Medium :" + sw.Elapsed);
            sw.Restart();
            ral.SaveLevel(@"C:\Users\Freddy\Desktop\level1.dlf", @"C:\Users\Freddy\Desktop\level1.llf", @"C:\Users\Freddy\Desktop\level1.fts", false);
            Console.WriteLine("Raw :" + sw.Elapsed);
            sw.Restart();
        }

        static void ProcGen()
        {
            var test = new RawArxLevel();
            var testMal = new MediumArxLevel();
            var testWdl = new WellDoneArxLevel();
            test.LoadLevel("level0");
            testWdl.LoadFrom(testMal.LoadFrom(test));
            var non_empty_cells = test.FTS.cells.Where(x => x.polygons.Length > 0).ToArray();

            var wdl = new WellDoneArxLevel();

            ////////////////////////////////////////////////////////////////////

            var procGenLevel = new ProcGenLevel();

            var floor = new FloorGenerator();

            floor.Size = new Vector2(5000, 5000);
            floor.Center = new Vector3(0, -100, 0);
            floor.TextureGenerator = new SingleTexture(@"graph\obj3d\textures\[soil]_human_standard2.jpg");

            procGenLevel.TerrainMeshes.Add(floor);

            var enhance = new DetailEnhancer();
            enhance.Shape = new Sphere() { Radius = 500, Falloff = 0 };

            procGenLevel.TerrainMeshModifiers.Add(enhance);

            var rumble = new Rumble();
            var noise = new SimplexNoiseValue();
            noise.Noise.Frequency = 0.002;// 0.0009;
            noise.Noise.OctaveCount = 2;
            rumble.NoiseValue = noise;
            rumble.Magnitude = 100;
            rumble.Shape = new Sphere() { Radius = 2000, Falloff = 1000 }.MultiplyWith(new FixedVector() { Value = new Vector3(0.1f, 1, 0.1f) });

            var lights = new RandomLightGenerator();
            lights.PositionShape = new FixedVector(new Vector3(0, -400, 0));
            lights.ColorShape = new FixedVector(new Vector3(1, 0.7f, 0.7f));
            lights.FalloffStart = new FixedValue(100);
            lights.FalloffEnd = new FixedValue(1000);
            lights.Intensity = new FixedValue(1);
            procGenLevel.LightGenerators.Add(lights);

            lights = new RandomLightGenerator();
            lights.Count = 200;
            lights.PositionShape = new Cuboid() { Min = new Vector3(-2500, -200, -2500), Max = new Vector3(2500, -200, 2500) };
            lights.ColorShape = new Cuboid() { Min = new Vector3(0.3f, 0.3f, 0.3f), Max = new Vector3(0.6f, 0.6f, 0.6f) };
            lights.FalloffStart = new RandomValue() { Min = 100, Max = 150 };
            lights.FalloffEnd = new RandomValue() { Min = 1500, Max = 2500 };
            lights.Intensity = new RandomValue() { Min = 0.1f, Max = 0.2f };
            procGenLevel.LightGenerators.Add(lights);

            procGenLevel.TerrainMeshModifiers.Add(rumble);

            procGenLevel.Apply(wdl);

            ////////////////////////////////////////////////////////////////////

            var mal = new MediumArxLevel();
            var ral = new RawArxLevel();
            wdl.SaveTo(mal);

            ArxLibertatisLightingCalculator.ArxLibertatisLightingCalculator.Calculate(mal, ArxLibertatisLightingCalculator.LightingProfile.DistanceAngleShadow);

            mal.DLF.header.positionEdit = new Vector3(0, -500, 0);
            mal.SaveTo(ral);
            ral.SaveLevel("level1", false);
        }

        public static void Main(string[] args)
        {
            ArxPaths.DataDir = @"F:\Program Files\Arx Libertatis";

            //ProcGen();
            //Console.ReadLine();
            //ScriptFunc.StartArxFatalis(3);
            ScriptFunc.Markdown = true;
            ScriptFunc.PrintPsDocs();
        }
    }
}
