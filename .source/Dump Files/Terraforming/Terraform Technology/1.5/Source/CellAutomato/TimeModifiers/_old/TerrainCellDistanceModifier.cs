using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class TerrainCellDistanceTimeModifier : TimeModifier
    {
        List<TerrainDef> terrainDefs;
        Verse.SimpleCurve factorCurve;
        float range;//maximum check range

        protected override int ModifyTime(IntVec3 center, Map map, int timeInput)
        {
            if (terrainDefs != null && terrainDefs.Count > 0 && range > 0)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                TerrainDef terrain;

                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);

                    if (curCenter.InBounds(map))
                    {
                        terrain = TerraformHelper.GetTerrain(map, curCenter);

                        if (terrainDefs.Contains(terrain))
                        {
                            return (int)(timeInput * factorCurve.Evaluate(curCenter.DistanceTo(center)));
                        }
                    }
                }
            }

            return timeInput;
        }
    }
}
