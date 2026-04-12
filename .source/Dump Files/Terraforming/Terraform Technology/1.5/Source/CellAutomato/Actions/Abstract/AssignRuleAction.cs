using RimWorld;
using System.Reflection;
using Verse;

namespace CellAutomato
{
    public class AssignRuleAction : RuleAction
    {
        TerrainCellRuleDef ruleDef = null;
        
        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (ruleDef != null)
            {
                if (chance < 1f)
                    if (chance > 0 && Rand.Chance(chance)) { }
                    else return;
                
                var cellGridComp = map.GetComponent<CellAutomatoGrid>();
                if(cellGridComp != null)
                {
                    cellGridComp.AssignRuleDefToCell(CellIndicesUtility.CellToIndex(center, map.Size.x), ruleDef);
                }
            }
        }
    }
}
