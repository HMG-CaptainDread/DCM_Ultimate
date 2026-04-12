using System.Collections.Generic;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    //test whether a cell in range has all of tags in mustinclude and no tags with mustnotinclude
    public class TerrainDistanceChecker : CheckerBase
    {
        public float range;
        public Success mode;
        public List<TerrainDef> terrainDefs;


        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            //Log.Message("TerrainDistanceChecker");
            if (terrainDefs != null && terrainDefs.Count > 0)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                TerrainDef terrain;
                
                if(mode == Success.Normal)
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);

                        if (curCenter.InBounds(map))
                        {
                            terrain = TerraformHelper.GetTerrain(map, curCenter);

                            if (terrainDefs.Contains(terrain))
                            {
                                return success == Success.Normal ? true : false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);

                        if (curCenter.InBounds(map))
                        {
                            terrain = TerraformHelper.GetTerrain(map, curCenter);

                            if (!terrainDefs.Contains(terrain))
                            {
                                return success == Success.Normal ? true : false;
                            }
                        }
                    }
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
