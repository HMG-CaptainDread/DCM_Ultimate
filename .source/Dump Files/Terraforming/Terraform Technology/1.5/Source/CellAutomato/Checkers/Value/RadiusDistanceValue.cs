using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class RadiusDistanceValue : CheckerRadiusNode
    {
        protected DistanceSelection selection = DistanceSelection.Closest;

        protected override float GetValue(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if(checker != null || checker2Point != null)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                float curAmount = 0;

                if(selection == DistanceSelection.Closest)
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);
                        if (curCenter.InBounds(map) && 
                            ((checker != null && checker.Check(curCenter, map, secondCheck, debug)) || 
                            (checker2Point != null && checker2Point.Check(center, curCenter, map, secondCheck, debug)))
                            )
                        {
                            return curCenter.DistanceTo(center);
                        }
                    }
                }
                else
                {
                    for (int i = num-1; i >= 0; --i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);
                        if (curCenter.InBounds(map) &&
                            ((checker != null && checker.Check(curCenter, map, secondCheck, debug)) ||
                            (checker2Point != null && checker2Point.Check(center, curCenter, map, secondCheck, debug)))
                            )
                        {
                            return curCenter.DistanceTo(center);
                        }
                    }
                }
            }

            return 0;
        }
    }
}
