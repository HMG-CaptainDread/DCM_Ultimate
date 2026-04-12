using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace CellAutomato
{
    public class RadiusActions : RuleAction
    {
        public List<RuleAction> actions;
        public float range;

        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (actions != null)
            {
                if (chance < 1f)
                    if (chance > 0 && Rand.Chance(chance)) { }
                    else return;
                
                int num = GenRadial.NumCellsInRadius(range);
                IntVec3 curCenter;
                
                for (int i = 0; i < num; ++i)
                {
                    curCenter = (center + GenRadial.RadialPattern[i]);
                    if (curCenter.InBounds(map))
                    {
                        foreach(var action in actions)
                        {
                            action.Apply(curCenter, map);
                        }
                    }
                }
            }
        }
    }
}
