using System.Collections.Generic;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    //test whether a cell in range has all of tags in mustinclude and no tags with mustnotinclude
    public class TerrainTagDistanceChecker : CheckerBase
    {
        public float range;
        public Success mode;
        public List<string> tags;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            //Log.Message("TerrainTagDistanceChecker");
            if ((tags != null && tags.Count > 0))
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                TerrainDef terrain;

                bool isSuccess;

                if (mode == Success.Normal)
                {
                    //terrain must include all tags in tags
                    for (int i = 0; i < num; ++i)
                    {
                        isSuccess = true;
                        curCenter = (center + GenRadial.RadialPattern[i]);
                        if (curCenter.InBounds(map))
                        {
                            terrain = TerraformHelper.GetTerrain(map, curCenter);

                            if (terrain.tags != null)
                            {
                                foreach (var includeTag in tags)
                                {
                                    if (!terrain.tags.Contains(includeTag))
                                    {
                                        isSuccess = false;
                                        break;
                                    }
                                }
                            }

                        }

                        if (isSuccess)
                            return success == Success.Normal ? true : false;
                    }
                }
                else
                {
                    for (int i = 0; i < num; ++i)
                    {
                        isSuccess = true;
                        curCenter = (center + GenRadial.RadialPattern[i]);
                        if (curCenter.InBounds(map))
                        {
                            terrain = TerraformHelper.GetTerrain(map, curCenter);

                            if (terrain.tags != null)
                            {
                                foreach (var notIncludeTag in tags)
                                {
                                    if (terrain.tags.Contains(notIncludeTag))
                                    {
                                        isSuccess = false;
                                        break;
                                    }
                                }
                            }
                        }

                        if (isSuccess)
                            return success == Success.Normal ? true : false;
                    }
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
