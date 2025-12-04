using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class TerrainDeteriorationRateTimeModifier : TimeModifier
    {
        Verse.SimpleCurve factorCurve;

        protected override int ModifyTime(IntVec3 center, Map map, int timeInput)
        {
            var terraindef = map.terrainGrid.TerrainAt(center);

            var deterioration = StatExtension.GetStatValueAbstract(terraindef, StatDefOf.DeteriorationRate);

            return (int)(timeInput * factorCurve.Evaluate(deterioration));
        }
    }
}
