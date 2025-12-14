using RimWorld;
using System.Collections.Generic;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class AcceptsFilthChecker : CheckerBase
    {
        public List<ThingDef> defs;
        
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (defs != null && defs.Count > 0)
            {
                foreach (var def in defs)
                {
                    if (FilthMaker.TerrainAcceptsFilth(map.terrainGrid.TerrainAt(center), def))
                    {
                        return success == Success.Normal ? true : false;
                    }
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
