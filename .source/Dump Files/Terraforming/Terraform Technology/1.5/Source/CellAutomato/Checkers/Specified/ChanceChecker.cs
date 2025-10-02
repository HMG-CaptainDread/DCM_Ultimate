using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class ChanceChecker : CheckerBase
    {
        float chance;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (secondCheck)
            {
                return success == Success.Normal ? true : false;
            }

            if (Rand.Chance(chance))
            {
                return success == Success.Normal ? true : false;
            }
            return success == Success.Normal ? false : true;
        }
    }
}
