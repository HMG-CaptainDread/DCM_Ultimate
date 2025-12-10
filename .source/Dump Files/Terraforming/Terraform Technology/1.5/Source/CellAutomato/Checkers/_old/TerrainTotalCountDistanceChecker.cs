using System.Collections.Generic;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    //checks whether total count is inside of any ranges
    public class TerrainTotalCountDistanceChecker : CheckerBase
    {
        public List<TerrainDef> checkList;
        public float range;
        public List<IntRange> countRanges;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            //Log.Message("TerrainTotalCountDistanceChecker");
            if (checkList != null && checkList.Count > 0)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                TerrainDef terrain;
                int curAmount = 0;
                
                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map))
                    {
                        terrain = TerraformHelper.GetTerrain(map, curCenter);
                        if (checkList.Contains(terrain))
                        {
                            ++curAmount;
                        }
                    }
                }
                
                if(success == Success.Normal)
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
