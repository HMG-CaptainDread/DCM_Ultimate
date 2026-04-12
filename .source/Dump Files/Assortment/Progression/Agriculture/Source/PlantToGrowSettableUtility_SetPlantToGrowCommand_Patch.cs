using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionAgriculture
{
	[HarmonyPatch(typeof(PlantToGrowSettableUtility), "SetPlantToGrowCommand")]
	public static class PlantToGrowSettableUtility_SetPlantToGrowCommand_Patch
	{
		public static bool Prefix(IPlantToGrowSettable settable, ref Command_SetPlantToGrow __result)
		{
			var plant = settable.GetPlantDefToGrow();
			if (plant is null)
			{
				IPlantToGrowSettable_GetPlantDefToGrow_Patch.allowVanilla = true;
				__result = new Command_SetPlantToGrowNothingToGrow()
				{
					defaultDesc = "PA.UnlockNewSeeds".Translate(),
					hotKey = KeyBindingDefOf.Misc12,
					settable = settable
				};
				IPlantToGrowSettable_GetPlantDefToGrow_Patch.allowVanilla = false;
				return false;
			}
			return true;
		}
	}
}
