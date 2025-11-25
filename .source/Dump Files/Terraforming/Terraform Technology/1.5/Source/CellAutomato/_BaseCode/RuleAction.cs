using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public abstract class RuleAction
    {
        private string patchTag;
        protected float chance = 1f;

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
