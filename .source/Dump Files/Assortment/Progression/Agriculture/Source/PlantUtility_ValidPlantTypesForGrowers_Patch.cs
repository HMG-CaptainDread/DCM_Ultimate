using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionAgriculture
{
    [HarmonyPatch(typeof(PlantUtility), "ValidPlantTypesForGrowers")]
    public static class PlantUtility_ValidPlantTypesForGrowers_Patch
    {
        public static void Postfix(ref IEnumerable<ThingDef> __result)
        {
            __result = __result.Where(x => GameComponent_UnlockedCrops.Instance.IsCropUnlocked(x));
        }
    }
}
