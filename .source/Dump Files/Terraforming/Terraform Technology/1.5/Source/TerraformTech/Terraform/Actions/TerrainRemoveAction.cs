using System.Reflection;
using Verse;

namespace TerraformTech
{
    public class TerrainRemoveAction : TerraformAction
    {
        public override string GetRuleNameString(TerrainTerraformRule rule)
        {
            var generatedLabel = string.Empty;
            var sourceDefs = rule.sourceDefs;

            if(sourceDefs != null)
            {
                for (int i = 0; i < rule.sourceDefs.Count; ++i)
                {
                    if (i > 0)
                    {
                        generatedLabel += ", ";
                    }
                    else if (i == 0)
                    {
                        generatedLabel += " ";
                    }

                    if (i > 2)
                    {
                        generatedLabel += "...";
                        break;
                    }
                    else
                    {
                        generatedLabel += rule.sourceDefs[i].label;
                    }
                }
            }
            else
            {
                generatedLabel += "?";
            }

            generatedLabel += " -> ∅/" + rule.resultDef.label;

            return generatedLabel;
        }


        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            TerrainDef underTerrain = TerraformHelper.GetUnderTerrain(map, center);

            if (underTerrain != null)
            {
                //if this is underbridge - we have no underterrain
                //if this is not bridge then we do not need to care about underterrain
                //crutches, crutches, crutches
                map.terrainGrid.RemoveTopLayer(center);
                //TerraformHelper.SetUnderTerrain(map, preferredResultUnderTerrainDef, center);
            }
            //else
            //if (preferredResultTerrainDef != null)
            //{
            //    TerraformHelper.SetTerrain(map, preferredResultTerrainDef, center);
                //TerraformHelper.SetUnderTerrain(map, preferredResultUnderTerrainDef, center);
            //}

            DoTerrainChangedEffects(center, map);
        }
    }
}
