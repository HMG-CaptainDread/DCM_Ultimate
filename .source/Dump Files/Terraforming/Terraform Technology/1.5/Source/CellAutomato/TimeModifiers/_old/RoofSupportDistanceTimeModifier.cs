using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class RoofSupportDistanceTimeModifier : TimeModifier
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
                //exclude self

                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);

                    if (curCenter.InBounds(map))
                    {
                        Building edifice = curCenter.GetEdifice(map);
                        if (!(edifice != null && edifice.def.holdsRoof))
                        {
                            continue;
                        }

                        float range = curCenter.DistanceTo(center);
                        return (int)(timeInput * factorCurve.Evaluate(range));
                    }
                }                
            }

            return timeInput;
        }
    }
}
