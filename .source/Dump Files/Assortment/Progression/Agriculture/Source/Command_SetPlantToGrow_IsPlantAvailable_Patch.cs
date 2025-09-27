using HarmonyLib;
using Verse;

namespace ProgressionAgriculture
{
    [HarmonyPatch(typeof(Command_SetPlantToGrow), "IsPlantAvailable")]
    public static class Command_SetPlantToGrow_IsPlantAvailable_Patch
    {
        public static void Postfix(ThingDef plantDef, ref bool __result)
        {
            if (GameComponent_UnlockedCrops.Instance.IsCropUnlocked(plantDef) is false) __result = false;
        }
    }
}
