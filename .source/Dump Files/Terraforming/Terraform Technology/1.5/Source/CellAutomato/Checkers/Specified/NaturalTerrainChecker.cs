using RimWorld;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class NaturalTerrainChecker : CheckerBase
    {
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            var terrain = TerraformHelper.GetTerrain(map, center);
            
            return success == Success.Normal ? terrain.natural : !terrain.natural;
        }
    }
}
