using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class BuildingChecker : CheckerBase
    {
        private List<Traversability> traversability;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            //Log.Message("BuildingOnTopChecker");
            if (traversability != null)
            {
                var building = center.GetFirstBuilding(map);
                if (building != null && traversability.Contains(building.def.passability))
                {
                    return success == Success.Normal ? true : false;
                }
            }
            else
            {
                if (center.GetFirstBuilding(map) != null)
                {
                    return success == Success.Normal ? true : false;
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
