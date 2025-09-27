using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class TerrainDeteriorationRateValue : CheckerBase
    {
        protected override float GetValue(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            var terraindef = map.terrainGrid.TerrainAt(center);

            return StatExtension.GetStatValueAbstract(terraindef, StatDefOf.DeteriorationRate);
        }
    }
}
