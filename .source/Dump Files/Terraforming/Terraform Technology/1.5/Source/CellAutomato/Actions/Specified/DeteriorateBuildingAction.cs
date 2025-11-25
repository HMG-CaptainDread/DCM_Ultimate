using RimWorld;
using System.Reflection;
using Verse;

namespace CellAutomato
{
    public class DeteriorateBuildingAction : RuleAction
    {
        public float power = 1;

        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            var building = center.GetFirstBuilding(map);

            SteadyEnvironmentEffects.DoDeteriorationDamage(building, center, map, false);
        }
    }
}
