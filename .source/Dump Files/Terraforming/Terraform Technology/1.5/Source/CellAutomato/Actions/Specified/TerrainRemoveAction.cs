using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class TerrainRemoveAction : RuleAction
    {
        public TerrainDef preferredResultTerrainDef = null;
        public TerrainDef preferredResultUnderTerrainDef = null;

        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (chance < 1f)
            if (chance > 0 && Rand.Chance(chance)) { }
                else return;

            TerrainDef underTerrain = TerraformHelper.GetUnderTerrain(map, center);

            if (underTerrain != null)
            {
                //if this is underbridge - we have no underterrain
                //if this is not bridge then we do not need to care about underterrain
                //crutches, crutches, crutches
                map.terrainGrid.RemoveTopLayer(center);
                TerraformHelper.SetUnderTerrain(map, preferredResultUnderTerrainDef, center);
            }
            else
            if (preferredResultTerrainDef != null)
            {
                TerraformHelper.SetTerrain(map, preferredResultTerrainDef, center);
                TerraformHelper.SetUnderTerrain(map, preferredResultUnderTerrainDef, center);
            }

            DoTerrainChangedEffects(center, map);
        }
    }
}
