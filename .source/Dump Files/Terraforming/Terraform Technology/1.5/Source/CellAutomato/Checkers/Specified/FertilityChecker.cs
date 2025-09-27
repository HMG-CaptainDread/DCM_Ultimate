using RimWorld;
using Verse;

namespace CellAutomato
{
    public class FertilityChecker : CheckerBase
    {
        float fertility = 0f;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (map.fertilityGrid.FertilityAt(center) > fertility)
            {
                return success == Success.Normal ? true : false;
            }

            return success == Success.Normal ? false : true;
        }
    }
}
