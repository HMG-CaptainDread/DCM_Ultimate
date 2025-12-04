using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System;

namespace ProgressionAgriculture
{
    [HarmonyPatch(typeof(ThingSetMaker), "Generate", new Type[] { typeof(ThingSetMakerParams) })]
    public static class ThingSetMaker_Generate_Patch
    {
        public static void Postfix(ThingSetMakerParams parms, ref List<Thing> __result)
        {
            if (SymbolResolver_LootScatter_Resolve_Patch.isLootGenerationGoingOn)
            {
                SymbolResolver_LootScatter_Resolve_Patch.isLootGenerationGoingOn = false;
                var count = Rand.RangeInclusive(1, 3);
                for (int i = 0; i < count; i++)
                {
                    var thing = DefGenerator_GenerateImpliedDefs_PreResolve_Patch.generatedBundles.RandomElement();
                    __result.Add(ThingMaker.MakeThing(thing));
                }
            }
        }
    }
}