using ArxLibertatisEditorIO.WellDoneIO;
using ArxLibertatisProcGenTools.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ArxLibertatisProcGenTools.Modifiers
{
    public class DetailEnhancer : IModifier
    {
        public IShape Shape { get; set; }

        private IEnumerable<Polygon> getEnhancedPolygons(Vertex a, Vertex b, Vertex c)
        {
            var ab_pos = (a.position + b.position) / 2;
            var ac_pos = (a.position + c.position) / 2;
            var bc_pos = (c.position + b.position) / 2;

            var ab_normal = (a.normal + b.normal) / 2;
            var ac_normal = (a.normal + c.normal) / 2;
            var bc_normal = (c.normal + b.normal) / 2;

            var ab_uv = (a.uv + b.uv) / 2;
            var ac_uv = (a.uv + c.uv) / 2;
            var bc_uv = (c.uv + b.uv) / 2;

            var ab_color = ((a.color + b.color) / 2).Clamped();
            var ac_color = ((a.color + c.color) / 2).Clamped();
            var bc_color = ((c.color + b.color) / 2).Clamped();

            var ab = new Vertex()
            {
                position = ab_pos,
                normal = ab_normal,
                uv = ab_uv,
                color = ab_color
            };

            var ac = new Vertex()
            {
                position = ac_pos,
                normal = ac_normal,
                uv = ac_uv,
                color = ac_color
            };

            var bc = new Vertex()
            {
                position = bc_pos,
                normal = bc_normal,
                uv = bc_uv,
                color = bc_color
            };

            Polygon p = new Polygon();
            p.vertices[0] = a.Copy();
            p.vertices[1] = ab.Copy();
            p.vertices[2] = ac.Copy();
            yield return p;

            p = new Polygon();
            p.vertices[0] = ab.Copy();
            p.vertices[1] = b.Copy();
            p.vertices[2] = bc.Copy();
            yield return p;

            p = new Polygon();
            p.vertices[0] = bc.Copy();
            p.vertices[1] = c.Copy();
            p.vertices[2] = ac.Copy();
            yield return p;

            p = new Polygon();
            p.vertices[0] = ab.Copy();
            p.vertices[1] = bc.Copy();
            p.vertices[2] = ac.Copy();
            yield return p;
        }

        public void Apply(WellDoneArxLevel wdl)
        {
            List<Polygon> toDelete = new List<Polygon>();
            List<Polygon> toAdd = new List<Polygon>();

            foreach (var polygon in wdl.polygons)
            {
                var isQuad = polygon.polyType.HasFlag(ArxLibertatisEditorIO.Util.PolyType.QUAD);

                var center = polygon.vertices[0].position;
                center += polygon.vertices[1].position;
                center += polygon.vertices[2].position;
                if (isQuad)
                {
                    center += polygon.vertices[3].position;
                }
                center /= isQuad ? 4 : 3;

                var affectedness = Shape.GetAffectedness(center);
                if (affectedness.X < 0.5f)
                {
                    continue;
                }

                var newPolygonsLazy = getEnhancedPolygons(polygon.vertices[0], polygon.vertices[1], polygon.vertices[2]);
                if (polygon.polyType == ArxLibertatisEditorIO.Util.PolyType.QUAD)
                {
                    newPolygonsLazy = newPolygonsLazy.Concat(getEnhancedPolygons(polygon.vertices[2], polygon.vertices[1], polygon.vertices[3]));
                }

                var newPolygons = newPolygonsLazy.ToArray();
                foreach (var newPolygon in newPolygons)
                {
                    newPolygon.polyType = polygon.polyType & ~ArxLibertatisEditorIO.Util.PolyType.QUAD; //remove quad type cause we only make triangles with this
                    newPolygon.room = polygon.room;
                    newPolygon.texturePath = polygon.texturePath;
                    newPolygon.transVal = polygon.transVal;
                    newPolygon.RecalculateArea();
                    newPolygon.RecalculateNormals();
                }

                toDelete.Add(polygon);
                toAdd.AddRange(newPolygons);
            }

            foreach (var polygon in toDelete)
            {
                wdl.polygons.Remove(polygon);
            }
            wdl.polygons.AddRange(toAdd);
        }
    }
}
