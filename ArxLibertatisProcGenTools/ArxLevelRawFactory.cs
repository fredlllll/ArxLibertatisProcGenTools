using ArxLibertatisEditorIO;
using ArxLibertatisEditorIO.IO.DLF;
using ArxLibertatisEditorIO.IO.FTS;
using ArxLibertatisEditorIO.IO.LLF;
using ArxLibertatisEditorIO.IO.Shared_IO;
using ArxLibertatisEditorIO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public class ArxLevelRawFactory
    {
        public ArxLevelRaw GetEmpty()
        {
            ArxLevelRaw lvl = new ArxLevelRaw();

            var dlf = lvl.DLF;
            var fts = lvl.FTS;
            var llf = lvl.LLF;

            //dlf header
            dlf.header = new DLF_IO_HEADER();
            dlf.header.version = 1.44f;
            dlf.header.identifier = Util.StringToChars("DANAE_FILE", 16);
            dlf.header.lastUser = Util.StringToChars("ArxLibertatisProcGenTools", 256);
            dlf.header.time = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            dlf.header.positionEdit = new SavedVec3() { x = 0, y = 0, z = 0 };
            dlf.header.angleEdit = new SavedAnglef() { a = 0, b = 0, g = 0 };
            dlf.header.numScenes = 1;
            dlf.header.numInters = 0;
            dlf.header.numNodes = 0;
            dlf.header.numNodelinks = 0;
            dlf.header.numZones = 0;
            dlf.header.lighting = 0;
            dlf.header.ipad1 = new int[256];
            dlf.header.numLights = 0;
            dlf.header.numFogs = 0;

            dlf.header.numBackgroundPolys = 0;
            dlf.header.numIgnoredPolys = 0;
            dlf.header.numChildPolys = 0;
            dlf.header.numPaths = 0;

            dlf.header.ipad2 = new int[250];
            dlf.header.offset = new SavedVec3() { x = 0, y = 0, z = 0 };
            dlf.header.fpad1 = new float[253];
            dlf.header.cpad1 = new byte[4096];
            dlf.header.ipad3 = new int[256];

            //scene
            var scenes = dlf.scenes = new DLF_IO_SCENE[1];
            scenes[0].name = IOHelper.GetBytes("Graph\\Levels\\unknown", 512);
            scenes[0].pad = new int[16];
            scenes[0].fpad = new float[16];

            //empty fields
            dlf.inters = new DLF_IO_INTER[0];
            dlf.fogs = new DLF_IO_FOG[0];
            dlf.lightColors = new uint[0];
            dlf.lightingHeader = new DANAE_IO_LIGHTINGHEADER();
            dlf.lights = new DANAE_IO_LIGHT[0];
            dlf.nodesData = new byte[0];
            dlf.paths = new DLF_IO_PATH[0];

            //fts
            fts.header = new FTS_IO_UNIQUE_HEADER();
            fts.header.path = Util.StringToChars("C:\\ARX\\Game\\Graph\\Levels\\unknown\\", 256);
            fts.header.count = 1;
            fts.header.version = 0.141f;
            fts.header.uncompressedsize = 0; //is calced on write by the io lib. 
            fts.header.pad = new int[3];

            fts.uniqueHeaders = new FTS_IO_UNIQUE_HEADER2[]
            {
                new FTS_IO_UNIQUE_HEADER2()
                {
                    path = Util.StringToChars("unknown.scn",256),
                    check = new byte[512]
                }
            };

            fts.sceneHeader = new FTS_IO_SCENE_HEADER()
            {
                version = 0.141f,
                sizex = 160,
                sizez = 160,
                nb_textures = 0,
                nb_polys = 0,
                nb_anchors = 0,
                nb_portals = 0,
                nb_rooms = 0, //should i add an empty room for starters?
                playerpos = new SavedVec3() { x = 0, y = 0, z = 0 },
                Mscenepos = new SavedVec3() { x = 0, y = 0, z = 0 }
            };

            fts.textureContainers = new FTS_IO_TEXTURE_CONTAINER[0];
            fts.cells = new FTS_IO_CELL[fts.sceneHeader.sizex * fts.sceneHeader.sizez];
            for (int i = 0; i < fts.cells.Length; ++i)
            {
                fts.cells[i].sceneInfo = new FTS_IO_SCENE_INFO()
                {
                    nbpoly = 0,
                    nbianchors = 0,
                };
                fts.cells[i].polygons = new FTS_IO_EERIEPOLY[0];
                fts.cells[i].anchors = new int[0];
            }
            fts.anchors = new FTS_IO_ANCHOR[0];
            fts.portals = new EERIE_IO_PORTALS[0];
            fts.rooms = new FTS_IO_ROOM[fts.sceneHeader.nb_rooms];
            for (int i = 0; i < fts.sceneHeader.nb_rooms; ++i)
            {
                fts.rooms[i].data = new EERIE_IO_ROOM_DATA
                {
                    nb_polys = 0,
                    nb_portals = 0,
                    padd = new int[6],
                };
                fts.rooms[i].portals = new int[0];
                fts.rooms[i].polygons = new FTS_IO_EP_DATA[0];
            }
            fts.roomDistances = new FTS_IO_ROOM_DIST_DATA[0];

            //llf
            llf.header = new LLF_IO_HEADER()
            {
                version = 1.44f,
                identifier = Util.StringToChars("DANAE_LLH_FILE", 16),
                lastuser = Util.StringToChars("ArxLibertatisProcGenTools", 256),
                time = (int)DateTimeOffset.Now.ToUnixTimeSeconds(),
                numLights = 0,
                numShadowPolys = 0,
                numIgnoredPolys = 0,
                numBackgroundPolys = 0,
                ipad1 = new int[256],
                fpad = new float[256],
                cpad = new byte[4096],
                ipad2 = new int[256]
            };
            llf.lights = new DANAE_IO_LIGHT[0];
            llf.lightingHeader = new DANAE_IO_LIGHTINGHEADER
            {
                numLights = 0,
                viewMode = 0,
                modeLight = 0,
                ipad = 0
            };
            llf.lightColors = new uint[0];

            return lvl;
        }
    }
}
