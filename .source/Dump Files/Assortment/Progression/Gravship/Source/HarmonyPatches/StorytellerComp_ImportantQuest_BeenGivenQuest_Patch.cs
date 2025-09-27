using HarmonyLib;
using RimWorld;
using Verse;
using System.Linq;
using System.Reflection;

namespace ProgressionGravship
{
    [HarmonyPatch(typeof(StorytellerComp_ImportantQuest), nameof(StorytellerComp_ImportantQuest.BeenGivenQuest), MethodType.Getter)]
    public static class StorytellerComp_ImportantQuest_BeenGivenQuest_Patch
    {
        public static void Postfix(StorytellerComp_ImportantQuest __instance, ref bool __result)
        {
            if (__instance.Props.questDef == QuestScriptDefOf.MechanoidSignal && Faction.OfPlayer.def.techLevel < TechLevel.Spacer)
            {
                __result = true;
            }
        }
    }
}
