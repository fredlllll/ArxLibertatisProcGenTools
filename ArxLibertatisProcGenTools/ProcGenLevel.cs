using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public class ProcGenLevel
    {
        public List<IMeshGenerator> Meshes { get; } = new List<IMeshGenerator>();
        //TODO: rooms/portals and entities and areas



        public void Apply(WellDoneArxLevel level)
        {
            foreach (IMeshGenerator mesh in Meshes)
            {
                level.polygons.AddRange(mesh.GetPolygons());
            }
        }
    }
}
