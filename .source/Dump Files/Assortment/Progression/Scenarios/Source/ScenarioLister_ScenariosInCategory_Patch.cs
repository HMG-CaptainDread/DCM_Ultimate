using System.Collections.Generic;
using RimWorld;
using HarmonyLib;
namespace ProgressionScenarios
{
	[HarmonyPatch(typeof(ScenarioLister), "ScenariosInCategory")]
	public static class ScenarioLister_ScenariosInCategory_Patch
	{
		public static TechLevel selectedLevel;
		public static IEnumerable<Scenario> Postfix(IEnumerable<Scenario> result)
		{
			foreach (var scenario in result)
			{
				if (selectedLevel == scenario.playerFaction.factionDef.techLevel)
				{
					yield return scenario;
				}
			}
		}
	}
}