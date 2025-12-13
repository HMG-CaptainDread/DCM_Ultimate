using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace ChickenExplosives
{
    public class PlaceWorker_ShowClaymoreAffectArea : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            CompProperties_ProjectileSprayer compProperties = def.GetCompProperties<CompProperties_ProjectileSprayer>();
            if (compProperties != null)
            {
                var cells = Utils.GetTotalAffectedCells(compProperties, rot, GenAdj.OccupiedRect(center, rot, def.size), Find.CurrentMap);
                GenDraw.DrawFieldEdges(cells.ToList(), Color.red);
            }
        }
    }
}