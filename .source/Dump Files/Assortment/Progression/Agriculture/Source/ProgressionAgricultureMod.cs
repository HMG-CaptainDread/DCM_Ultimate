using HarmonyLib;
using UnityEngine;
using Verse;

namespace ProgressionAgriculture
{
	public class ProgressionAgricultureMod : Mod
	{
		public static ProgressionAgricultureModSettings settings;
		public ProgressionAgricultureMod(ModContentPack pack) : base(pack)
		{
			new Harmony("ProgressionAgricultureMod").PatchAll();
			settings = GetSettings<ProgressionAgricultureModSettings>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);
			settings.DoSettingsWindowContents(inRect);
		}

        public override string SettingsCategory()
        {
            return Content.Name;
        }
	}
}