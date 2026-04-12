using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionAgriculture
{
    [HarmonyPatch(typeof(PlayerItemAccessibilityUtility), "PossiblyAccessible")]
    public static class PlayerItemAccessibilityUtility_PossiblyAccessible_Patch
    {
        public static void Postfix(ThingDef thing, int count, Map map, ref bool __result)
        {
            var comp = thing.GetCompProperties<CompProperties_UnlockCrop>();
            if (comp != null)
            {
                __result = false;
            }
        }
    }
}