using System.Reflection;
using Verse;

namespace TerraformTech
{
    public class TerrainPlaceOnTopAction : TerraformAction
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
            generatedLabel += " ⩲ " + rule.resultDef.label;

            return generatedLabel;
        }


        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            TerrainDef willBeUnderTerrain = TerraformHelper.GetTerrain(map, center);

            TerraformHelper.SetTerrain(map, preferredResultTerrainDef, center);
            TerraformHelper.SetUnderTerrain(map, willBeUnderTerrain, center);
            
            DoTerrainChangedEffects(center, map);
        }
    }
}
