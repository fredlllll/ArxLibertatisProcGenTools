using ArxLibertatisEditorIO;
using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.RawIO;
using ArxLibertatisEditorIO.Util;
using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.MeshGens;
using System;
using System.Linq;
using System.Numerics;

namespace ArxLibertatisProcGenToolsTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            ArxPaths.DataDir = @"F:\Program Files\Arx Libertatis";

            var ral = new RawArxLevel();
            ral.LoadLevel("level1");
            var mal = new MediumArxLevel();
            mal.LoadLevel(ral);
            var wdl = new WellDoneArxLevel();
            wdl.LoadLevel(mal);


            //monkepos 8550 3050 8800

            OBJImporter o = new OBJImporter(@"C:\Users\Freddy\Desktop\monke.obj", @"C:\Users\Freddy\Desktop\monke.mtl");
            o.room = 40;

            Vector3 pos = new Vector3(8550, -3050, 8800);

            var translation = Matrix4x4.CreateTranslation(pos);
            var scale = Matrix4x4.CreateScale(100);
            o.worldMatrix = scale*translation;

            wdl.polygons.AddRange(o.GetPolygons());

            wdl.SaveLevel(mal);
            mal.SaveLevel(ral);
            ral.SaveLevel();
        }
    }
}
