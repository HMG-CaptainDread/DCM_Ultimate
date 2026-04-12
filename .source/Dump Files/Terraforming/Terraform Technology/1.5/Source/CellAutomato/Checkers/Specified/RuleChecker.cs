using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class RuleChecker : CheckerBase
    {
        protected List<TerrainCellRuleDef> defs;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            var ruleGrid = map.GetComponent<CellAutomatoGrid>();
            var rules = ruleGrid.GetRulesAtCell(map.cellIndices.CellToIndex(center));
            if (rules.Count > 0 && defs != null && defs.Count > 0)
            {
                foreach(var rule in defs)
                    if(rules.Any(r=> r.defRuleActivated == rule.defName))
                    {
                        return success == Success.Normal ? true : false;
                    }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
