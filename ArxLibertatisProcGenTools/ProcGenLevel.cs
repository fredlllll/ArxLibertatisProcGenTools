using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.Generators;
using ArxLibertatisProcGenTools.Modifiers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public class ProcGenLevel
    {
        public List<IMeshGenerator> TerrainMeshes { get; } = new List<IMeshGenerator>();
        public List<IModifier> TerrainMeshModifiers { get; } = new List<IModifier>();
        //TODO: rooms/portals and entities and areas

        public List<ILightGenerator> LightGenerators { get; } = new List<ILightGenerator>();
        public List<Light> Lights { get; } = new List<Light>();


        public void Apply(WellDoneArxLevel level)
        {
            foreach (IMeshGenerator mesh in TerrainMeshes)
            {
                level.polygons.AddRange(mesh.GetPolygons());
            }

            foreach (var mod in TerrainMeshModifiers)
            {
                mod.Apply(level);
            }

            foreach (var lightGen in LightGenerators)
            {
                level.lights.AddRange(lightGen.GetLights());
            }

            level.lights.AddRange(Lights);
        }
    }
}
