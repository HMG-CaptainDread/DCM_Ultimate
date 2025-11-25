using System.Numerics;
using HarmonyLib;
using Verse;
using Verse.Sound;

namespace ProgressionScenarios
{
	public class ProgressionScenariosMod : Mod
	{
		public ProgressionScenariosMod(ModContentPack pack) : base(pack)
		{
			new Harmony("ProgressionScenariosMod").PatchAll();
		}
	}
}