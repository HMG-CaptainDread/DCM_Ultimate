using System.Collections.Generic;
using Verse;
using System.Linq;
using TerraformTech;

namespace CellAutomato
{

    public class TerrainCountDistanceCheckerAll : CheckerBase
    {
        protected static Dictionary<TerrainDef, int> terrainCounts = new Dictionary<TerrainDef, int>();

        public List<TerrainCountRangeClass> checkList;
        public float range;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            //Log.Message("TerrainCountDistanceCheckerAll");
            if (checkList != null && checkList.Count > 0)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                TerrainDef terrain;
                int curAmount = 0;

                terrainCounts.Clear();
                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map))
                    {
                        terrain = TerraformHelper.GetTerrain(map, curCenter);
                        if (terrainCounts.ContainsKey(terrain))
                        {
                            terrainCounts[terrain] += 1;
                        }
                        else
                        {
                            terrainCounts[terrain] = 1;
                        }
                    }
                }

                curAmount = 0;
                //must satisfy all conditions
                foreach (var terrainClass in checkList)
                {
                    terrainCounts.TryGetValue(terrainClass.TerrainDef, out curAmount);
                    if(terrainClass.CountRange.min <= terrainClass.CountRange.max)
                    {
                        if (!(terrainClass.CountRange.min <= curAmount && curAmount <= terrainClass.CountRange.max))
                        {
                            return success == Success.Normal ? false : true;
                        }
                    }
                    else
                    {
                        if (!(terrainClass.CountRange.min < curAmount || curAmount < terrainClass.CountRange.max))
                        {
                            return success == Success.Normal ? false : true;
                        }
                    }
                }
            }

            return success == Success.Normal ? true : false;
        }
    }
}
