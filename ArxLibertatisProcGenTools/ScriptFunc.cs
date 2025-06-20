using ArxLibertatisEditorIO;
using ArxLibertatisEditorIO.MediumIO;
using ArxLibertatisEditorIO.RawIO;
using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisLightingCalculatorLib;
using ArxLibertatisProcGenTools.Generators;
using ArxLibertatisProcGenTools.Modifiers;
using System;
using System.Numerics;

namespace ArxLibertatisProcGenTools
{
    public static partial class ScriptFunc
    {
        private static WellDoneArxLevel? level = null;
        private static LightingProfile lightingProfile = LightingProfile.Danae;
        private static Vector3 playerStartPosition = Vector3.Zero;

        public static WellDoneArxLevel Level
        {
            get
            {
                level ??= new WellDoneArxLevel();
                return level;
            }
        }

        [Description("deletes the level currently in memory. required cause its persistent during the powershell session")]
        public static void Clear()
        {
            Console.WriteLine("Clearing Data");
            level = null;
            lightingProfile = LightingProfile.Danae;
            playerStartPosition = Vector3.Zero;
        }

        [Description("Set data dir that the level is saved to (and can be loaded from)")]
        public static void SetDataDir(string dataDir)
        {
            Console.WriteLine("Setting Data Dir");
            ArxPaths.DataDir = dataDir;
        }

        [Description("Loads a level to be modified")]
        public static void LoadLevel(string name)
        {
            Console.WriteLine("Loading Level");
            var lvl = new RawArxLevel();
            var mal = new MediumArxLevel();
            level = new WellDoneArxLevel();
            lvl.LoadLevel(name);
            level.LoadFrom(mal.LoadFrom(lvl));
        }

        [Description("Saves the level")]
        public static void SaveLevel(string name)
        {
            Console.WriteLine("Saving Level");
            var mal = new MediumArxLevel();
            var ral = new RawArxLevel();
            Level.SaveTo(mal);

            ArxLibertatisLightingCalculator.Calculate(mal, lightingProfile);

            mal.DLF.header.positionEdit = playerStartPosition;
            mal.SaveTo(ral);
            ral.SaveLevel(name, true);
        }

        [Description("Applies the Mesh Generator to the level")]
        public static void Apply(IMeshGenerator meshGenerator)
        {
            Console.WriteLine("Applying Mesh Generator");
            Level.polygons.AddRange(meshGenerator.GetPolygons());
        }

        [Description("Applies the Modifier to the level")]
        public static void Apply(IModifier meshModifier)
        {
            Console.WriteLine("Applying Modifier");
            meshModifier.Apply(Level);
        }

        [Description("Applies the Light Generator to the level")]
        public static void Apply(ILightGenerator lightGenerator)
        {
            Console.WriteLine("Applying Light Generator");
            Level.lights.AddRange(lightGenerator.GetLights());
        }

        public static void SetLightingProfile(LightingProfile lightingProfile)
        {
            Console.WriteLine("Setting Lighting Profile");
            ScriptFunc.lightingProfile = lightingProfile;
        }

        public static void SetPlayerStart(Vector3 position)
        {
            Console.WriteLine("Setting Player Start Position");
            playerStartPosition = position;
        }
    }
}
