namespace ProgressionAgriculture
{
	using UnityEngine;
	using Verse;

	public class ProgressionAgricultureModSettings : ModSettings
	{
		public static int RecipeDefIngredientCount = 10;
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref RecipeDefIngredientCount, "RecipeDefIngredientCount", 10);
		}

		public void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard listingStandard = new Listing_Standard();
			listingStandard.Begin(inRect);
			var sliderValue = listingStandard.SliderLabeled("PA.SeedBundleIngredientCount".Translate
				(RecipeDefIngredientCount), RecipeDefIngredientCount, 1f, 100f);
			RecipeDefIngredientCount = Mathf.RoundToInt(sliderValue);
			listingStandard.End();
		}
	}
}