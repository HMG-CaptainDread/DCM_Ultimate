using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    //test whether a cell in range has all of tags in mustinclude and no tags with mustnotinclude
    public class PlantDefTotalCountDistanceChecker : CheckerBase
    {
        public float range;
        public List<ThingDef> checkList;
        public List<IntRange> countRanges;
        
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (checkList != null && checkList.Count > 0)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                Plant plant;
                int curAmount = 0;

                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map))
                    {
                        plant = GridsUtility.GetPlant(curCenter, map);

                        if (plant != null && checkList.Contains(plant.def))
                        {
                            ++curAmount;
                        }
                    }
                }

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
