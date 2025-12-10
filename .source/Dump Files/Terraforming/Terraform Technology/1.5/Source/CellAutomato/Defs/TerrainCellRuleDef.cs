using System.Collections.Generic;
using Verse;

namespace CellAutomato
{

    public class TerrainCellRuleDef : Def
    {
        public bool isNatural;
        public IntRange time;
        public int priority;
        public TerrainSelector cellSelector;
        public CellLayerDef layer;

        public List<TimeModifier> timeModifiers;
        public List<CheckerBase> conditions;
        public CheckerBase checker;
        public List<RuleAction> actions;
        public List<RuleAction> failActions;
    }

}
