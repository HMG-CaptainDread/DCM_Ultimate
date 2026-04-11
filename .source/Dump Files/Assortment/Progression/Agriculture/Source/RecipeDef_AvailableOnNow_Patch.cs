using HarmonyLib;
using Verse;
using System.Linq;

namespace ProgressionAgriculture
{
	[HarmonyPatch(typeof(RecipeDef), "AvailableOnNow")]
	public static class RecipeDef_AvailableOnNow_Patch
	{
		public static bool Prefix(RecipeDef __instance, Thing thing, ref bool __result)
		{
			if (typeof(RecipeWorker_Seed).IsAssignableFrom(__instance.workerClass)
			 && (__instance.PotentiallyMissingIngredients(null, thing.MapHeld).Any() 
			 || GameComponent_UnlockedCrops.Instance.IsCropUnlocked(__instance.ProducedThingDef
			 	.GetCompProperties<CompProperties_UnlockCrop>().cropDef)))
			{
				__result = false;
				return false;
			}
			return true;
		}
	}
}