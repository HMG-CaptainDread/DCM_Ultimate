using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class AdjacentRoofCountTimeModifier : TimeModifier
    {
        Verse.SimpleCurve factorCurve;
        float range;//maximum check range

        protected override int ModifyTime(IntVec3 center, Map map, int timeInput)
        {
            if (range > 0)
            {
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                TerrainDef terrain;
                int roofCount = 0;
                //exclude self

                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    
                    if (curCenter.InBounds(map) && map.roofGrid.Roofed(curCenter)) ++roofCount;
                }

                if (map.roofGrid.Roofed(center)) --roofCount;
                
                return (int)(timeInput * factorCurve.Evaluate(roofCount));
            }

            return timeInput;
        }
    }
}
