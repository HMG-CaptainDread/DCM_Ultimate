using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class PlantContainerChecker : CheckerBase
    {
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            List<Thing> list = map.thingGrid.ThingsListAt(center);
            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is Building_PlantGrower)
                {
                    flag = true;
                    break;
                }
            }

            return success == Success.Normal ? flag : !flag;
        }
    }
}
