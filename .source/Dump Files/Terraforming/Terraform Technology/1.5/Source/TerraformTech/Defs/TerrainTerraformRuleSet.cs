using System;
using System.Collections.Generic;
using Verse;

namespace TerraformTech
{
    //packed ruleset def which is unpacked as a bunch of TerrainTerraformDef
    public class TerrainTerraformRuleSet : ThingDef
    {
        public List<TerrainTerraformRule> rules = null;
        public Type defaultRuleActionClass;

        //public override void PostLoad()
        //{
        //    if(defaultRuleActionClass == null || !(typeof(RuleAction).IsAssignableFrom(defaultRuleActionClass)))
        //    {
        //        defaultRuleActionClass = typeof(TerrainReplaceAction);
        //    }
        //}

    }
}
