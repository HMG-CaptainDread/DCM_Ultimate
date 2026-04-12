using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionAgriculture
{
	[HarmonyPatch(typeof(PlantUtility), "CanSowOnGrower")]
	public static class PlantUtility_CanSowOnGrower_Patch
	{
		public static void Postfix(ThingDef plantDef, object obj, ref bool __result)
		{
			if (GameComponent_UnlockedCrops.Instance.IsCropUnlocked(plantDef) is false) __result = false;
		}
	}
}
