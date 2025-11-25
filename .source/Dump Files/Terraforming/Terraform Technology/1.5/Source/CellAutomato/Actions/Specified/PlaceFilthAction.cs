using RimWorld;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class PlaceFilthAction : RuleAction
    {
        public ThingDef def = null;
        public int count = 1;

        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (chance < 1f)
                if (chance > 0 && Rand.Chance(chance)) { }
                else return;
            
            if(def != null && count > 0)
            {
                bool result = FilthMaker.TryMakeFilth(center, map, def, count);

                Log.Message("Filth result " + result);
            }
        }
    }
}
