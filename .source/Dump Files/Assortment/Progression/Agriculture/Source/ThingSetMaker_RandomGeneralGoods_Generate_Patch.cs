using HarmonyLib;
using RimWorld.BaseGen;

namespace ProgressionAgriculture
{
	[HarmonyPatch(typeof(SymbolResolver_LootScatter), "Resolve")]
	public static class SymbolResolver_LootScatter_Resolve_Patch
	{
		public static bool isLootGenerationGoingOn;
		public static void Prefix()
		{
			isLootGenerationGoingOn = true;
		}
	}
}