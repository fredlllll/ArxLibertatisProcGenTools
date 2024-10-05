using ArxLibertatisEditorIO;
using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.RawIO;
using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools;
using ArxLibertatisProcGenTools.Generators.Mesh;
using ArxLibertatisProcGenTools.Generators.Plane;
using ArxLibertatisProcGenTools.Generators.Texture;
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
            o.room = 40;

            Vector3 pos = new Vector3(8550, -3050, 8800);

            var translation = Matrix4x4.CreateTranslation(pos);
            var scale = Matrix4x4.CreateScale(100);
            o.worldMatrix = scale * translation;

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

            var procGenLevel = new ProcGenLevel();

            var floor = new FloorGenerator();

            floor.Size = new Vector2(1000, 1000);
            floor.TextureGenerator = new SingleTexture(@"graph\obj3d\textures\[soil]_human_standard2.jpg");

            procGenLevel.Meshes.Add(floor);
            procGenLevel.Apply(wdl);

            var mal = new MediumArxLevel();
            var ral = new RawArxLevel();
            wdl.SaveTo(mal);
            mal.DLF.header.positionEdit = new Vector3(0, -300, 0);
            mal.SaveTo(ral);
            var non_empty_cells_2 = ral.FTS.cells.Where(x => x.polygons.Length > 0).ToArray();
            ral.SaveLevel("level1", false);
            /*ral.SaveLevel(@"F:\Program Files\Arx Libertatis\graph\levels\level1\level1.dlf",
                          @"F:\Program Files\Arx Libertatis\graph\levels\level1\level1.llf",
                          @"F:\Program Files\Arx Libertatis\game\graph\levels\level1\level1.fts",
                          false);*/
        }

        public static void Main(string[] args)
        {
            ArxPaths.DataDir = @"F:\Program Files\Arx Libertatis";

            ProcGen();
        }
    }
}
