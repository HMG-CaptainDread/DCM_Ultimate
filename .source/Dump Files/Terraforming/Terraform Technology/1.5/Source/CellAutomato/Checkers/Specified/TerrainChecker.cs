using System.Collections.Generic;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    //checks whether total count is inside of any ranges
    public class TerrainChecker : CheckerBase
    {
        public List<TerrainDef> defs;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (defs != null && defs.Count > 0)
            {
                var terrain = TerraformHelper.GetTerrain(map, center);
                if (defs.Contains(terrain))
                {
                    return success == Success.Normal ? true : false;
                }
                else
                {
                    return success == Success.Normal ? false : true;
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
