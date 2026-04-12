using UnityEngine;
using Verse;

namespace TerraformTech
{
    public class PlaceWorker_Terraforming : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            base.DrawGhost(def, center, rot, ghostCol, thing);
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            if (def is TerrainTerraformDef terraDef)
            {
                TerrainDef terrain = TerraformHelper.GetTerrain(map, center);

                if (terrain != null)
                {
                    if (terraDef.terraformRule.sourceDefs.Contains(terrain)) return true;
                }
                return false;
            }
            else
            if (def is TerrainTerraformRuleSet ruleSetDef)
            {
                TerrainDef terrain = TerraformHelper.GetTerrain(map, center);

                if (terrain != null)
                {
                    foreach (var ruleDef in ruleSetDef.rules)
                    {
                        if (ruleDef.sourceDefs.Contains(terrain))
                        {
                            return true;
                        }
                    }
                }
                return false;
            } 

            return true;  
        }  
    }
}
