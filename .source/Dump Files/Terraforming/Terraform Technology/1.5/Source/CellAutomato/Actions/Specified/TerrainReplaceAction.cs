using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class TerrainReplaceAction : RuleAction
    {
        public TerrainDef preferredResultTerrainDef = null;
        public TerrainDef preferredResultUnderTerrainDef = null;

        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (chance < 1f)
                if (chance > 0 && Rand.Chance(chance)) { }
                else return;
            //this is because underterrain may get deleted.
            var underTerrain = TerraformHelper.GetUnderTerrain(map, center);
            if (preferredResultTerrainDef != null)
            {
                TerraformHelper.SetTerrain(map, preferredResultTerrainDef, center);
                //map.terrainGrid.SetTerrain(center, preferredResultTerrainDef);
            }

            if (underTerrain != null)
            {
                TerraformHelper.SetUnderTerrain(map, underTerrain, center);
                //map.terrainGrid.SetUnderTerrain(center, underTerrain);
            }
            else
            if (preferredResultUnderTerrainDef != null)
            {
                TerraformHelper.SetUnderTerrain(map, preferredResultUnderTerrainDef, center);
                //map.terrainGrid.SetUnderTerrain(center, preferredResultUnderTerrainDef);
            }


            DoTerrainChangedEffects(center, map);
        }
    }
}
