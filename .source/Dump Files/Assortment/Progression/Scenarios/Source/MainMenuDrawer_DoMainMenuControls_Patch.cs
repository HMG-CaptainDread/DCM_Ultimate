using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionScenarios
{
	[HarmonyPatch(typeof(MainMenuDrawer), "DoMainMenuControls")]
	public static class MainMenuDrawer_DoMainMenuControls_Patch
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
		{
			var drawOptionListingInfo = AccessTools.Method(typeof(OptionListingUtility), "DrawOptionListing");
			foreach (var codeInstruction in codeInstructions)
			{
				if (codeInstruction.Calls(drawOptionListingInfo))
				{
					yield return new CodeInstruction(OpCodes.Ldloc_2);
					yield return new CodeInstruction(OpCodes.Call,
						AccessTools.Method(typeof(MainMenuDrawer_DoMainMenuControls_Patch), "ChangeNewColonyButton"));
				}
				yield return codeInstruction;
			}
		}

		public static void ChangeNewColonyButton(List<ListableOption> list)
		{
			var newColony = list.FindIndex(x => x.label == "NewColony".Translate());
			if (newColony >= 0)
			{
				list.RemoveAt(newColony);
				list.Insert(newColony, new ListableOption("NewColony".Translate(), delegate
				{
					Find.WindowStack.Add(new Page_SelectTechLevel());
				}));
			}
		}
	}
}