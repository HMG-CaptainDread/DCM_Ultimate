using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ProgressionAgriculture
{
	[HarmonyPatch(typeof(TransferableUIUtility), "DoExtraIcons")]
	public static class TransferableUIUtility_DoExtraIcons_Patch
	{
		private static float IconWidth = 24f;
		public static void Postfix(Transferable trad, Rect rect, ref float curX)
		{
			var props = trad?.ThingDef?.GetCompProperties<CompProperties_UnlockCrop>();
			if (props != null)
			{
				bool unlocked = GameComponent_UnlockedCrops.Instance.IsCropUnlocked(props.cropDef);
				if (unlocked)
				{
					var iconRect = new Rect(curX - IconWidth, (rect.height - IconWidth) / 2f, IconWidth, IconWidth);
					GUI.DrawTexture(iconRect, Widgets.GetCheckboxTexture(true));
					curX -= IconWidth;
					TooltipHandler.TipRegion(iconRect, "PA.YouUnlockedSeed".Translate());
				}
			}
		}
	}
}