using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class RadiusCountValue : CheckerRadiusNode
    {
        protected override float GetValue(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            int num = GenRadial.NumCellsInRadius(range);
            IntVec3 curCenter;
            float curAmount = 0;

            if (checker != null)
            {
                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map) &&
                        (checker2Point != null && checker2Point.Check(center, curCenter, map, secondCheck, debug))
                        )
                    {
                        curAmount += checker.Get(curCenter, map, secondCheck, debug);
                        return curCenter.DistanceTo(center);
                    }
                }
            }
            else
            if (checker2Point != null)
            {
                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map) &&
                        (checker2Point != null && checker2Point.Check(center, curCenter, map, secondCheck, debug))
                        )
                    {
                        curAmount += 1;
                        return curCenter.DistanceTo(center);
                    }
                }
            }

            return curAmount;
        }

    }
}
