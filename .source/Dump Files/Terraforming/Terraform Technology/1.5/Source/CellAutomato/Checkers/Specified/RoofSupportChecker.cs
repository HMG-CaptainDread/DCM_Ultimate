using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class RoofSupportChecker : CheckerBase
    {
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            var support = GridsUtility.GetRoofHolderOrImpassable(center, map);
            if (support != null)
            {
                return success == Success.Normal ? true : false;
            }

            return success == Success.Normal ? false : true;
        }
    }
}
