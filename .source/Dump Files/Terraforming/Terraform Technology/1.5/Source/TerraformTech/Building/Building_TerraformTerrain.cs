using RimWorld;
using Verse;

namespace TerraformTech
{
    public class Building_TerraformTerrain : Building
    {  
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if(def is TerrainTerraformRuleSet)
            {
                var ruleSetDef = (TerrainTerraformRuleSet)def;

                TerrainDef terrain = TerraformHelper.GetTerrain(map, Position);
                TerrainTerraformDef defTerra = null;
                    
                foreach (var ruleDef in ruleSetDef.rules)
                {
                    if (ruleDef.sourceDefs.Contains(terrain))
                    {
                        defTerra = ruleDef.terraformDef;
                    }
                }

                if(defTerra!= null)
                {
                    GenConstruct.PlaceBlueprintForBuild(defTerra, this.Position, this.Map, this.Rotation, this.Faction, this.Stuff);
                }
            }
            else
            if (def is TerrainTerraformDef)
            {
                var defTerra = (TerrainTerraformDef)def;

                defTerra.terraformRule.action.Apply(this.Position, this.Map);

                if (defTerra.terraformRule.resultList != null)
                    for (int k = 0; k < defTerra.terraformRule.resultList.Count; k++)
                    {
                        ThingDefCountClass thingDefCountClass = defTerra.terraformRule.resultList[k];
                        Thing thing = ThingMaker.MakeThing(stuff: null, def: thingDefCountClass.thingDef);
                        thing.stackCount = thingDefCountClass.count;

                        GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.VanishOrMoveAside);
                    }
            }

            this.Destroy(DestroyMode.Vanish);
        }
    }
}
