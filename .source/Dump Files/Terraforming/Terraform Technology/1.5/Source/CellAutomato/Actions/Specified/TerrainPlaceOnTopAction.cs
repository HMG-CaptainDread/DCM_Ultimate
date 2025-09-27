using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class TerrainPlaceOnTopAction : RuleAction
    {
        public TerrainDef preferredResultTerrainDef = null;
        public TerrainDef preferredResultUnderTerrainDef = null;

        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (chance < 1f)
                if (chance > 0 && Rand.Chance(chance)) { }
                else return;

            TerrainDef willBeUnderTerrain = TerraformHelper.GetTerrain(map, center);

            TerraformHelper.SetTerrain(map, preferredResultTerrainDef, center);
            TerraformHelper.SetUnderTerrain(map, willBeUnderTerrain, center);
            
            DoTerrainChangedEffects(center, map);
        }
    }
}
