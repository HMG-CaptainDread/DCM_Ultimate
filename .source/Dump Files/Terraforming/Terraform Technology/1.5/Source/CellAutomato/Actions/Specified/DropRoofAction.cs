using RimWorld;
using System.Reflection;
using Verse;

namespace CellAutomato
{
    public class DropRoofAction : RuleAction
    {        
        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if(map.roofGrid.Roofed(center))
                RoofCollapserImmediate.DropRoofInCells(center, map);
        }
    }
}
