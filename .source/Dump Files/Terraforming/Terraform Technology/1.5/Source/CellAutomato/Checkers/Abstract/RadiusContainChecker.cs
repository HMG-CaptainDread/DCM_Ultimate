using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class RadiusContainChecker : CheckerRadiusNode
    {
        protected Mode mode = Mode.Single;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (checker != null || checker2Point != null)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;

                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (debug)
                    {
                        Log.Message("[RadiusChecker] check at cell [" + curCenter + "]");
                    }

                    if (curCenter.InBounds(map) && 
                        ((checker != null && checker.Check(curCenter, map, secondCheck, debug)) || 
                         (checker2Point != null && checker2Point.Check(center, curCenter, map, secondCheck, debug)))
                        )
                    {
                        if(mode == Mode.Single)
                        {
                            return success == Success.Normal ? true : false;
                        }
                    }
                    else
                    {
                        if (mode == Mode.All)
                        {
                            return success == Success.Normal ? false : true;
                        }
                    }
                }
            }

            if (mode == Mode.Single)
            {
                return success == Success.Normal ? false : true;
            }
            else
            {
                return success == Success.Normal ? true : false;
            }
        }
        
        protected override float GetValue(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (checker != null)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                float curAmount = 0;

                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map))
                    {
                        curAmount += checker.Get(center, map, secondCheck, debug);
                    }
                }
            }

            return 0;
        }
    }
}
