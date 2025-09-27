using RimWorld;
using System.Collections.Generic;
using Verse;

namespace TerraformTech
{
    public abstract class TerraformAction
    {
        public TerrainDef preferredResultTerrainDef = null;
        //public TerrainDef preferredResultUnderTerrainDef = null;

        protected virtual void ApplyRule(IntVec3 center, Map map)
        {

        }
        public void Apply(IntVec3 center, Map map)
        {
            ApplyRule(center, map);
        }

        public void Apply(int center, Map map)
        {
            ApplyRule(Verse.CellIndicesUtility.IndexToCell(center, map.Size.x), map);
        }

        public virtual string GetRuleNameString(TerrainTerraformRule rule)
        {
            var generatedLabel = string.Empty;
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
            generatedLabel += " -> " + rule.resultDef.label;

            return generatedLabel;
        }

        public virtual void DoTerrainChangedEffects(IntVec3 center, Map map)
        {
            //List<Thing> thingList = center.GetThingList(map);

            //for (int num = thingList.Count - 1; num >= 0; num--)
            //{
            //    if (thingList[num].def.category == ThingCategory.Building && !GenConstruct.CanBuildOnTerrain(thingList[num].def, thingList[num].Position, map, thingList[num].Rotation, null, thingList[num].Stuff))
            //    {
            //        thingList[num].Destroy(DestroyMode.KillFinalize);
            //    }
            //}
        }
    }
}
