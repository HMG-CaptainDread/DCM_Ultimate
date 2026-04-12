using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class CounterChecker : CheckerBase
    {
        protected List<IntRange> countRanges;
        protected CheckerBase checker;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (countRanges != null && countRanges.Count > 0)
            {
                int curAmount = (int)checker.Get(center, map);
                if (success == Success.Normal)
                {
                    //fit into any countRange
                    foreach (var countRange in countRanges)
                    {
                        if (countRange.min <= countRange.max)
                        {
                            if (countRange.min <= curAmount && curAmount <= countRange.max)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (countRange.min < curAmount || curAmount < countRange.max)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    //if fits into any countRange - return false
                    foreach (var countRange in countRanges)
                    {
                        if (countRange.min <= countRange.max)
                        {
                            if (countRange.min <= curAmount && curAmount <= countRange.max)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (countRange.min < curAmount || curAmount < countRange.max)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
