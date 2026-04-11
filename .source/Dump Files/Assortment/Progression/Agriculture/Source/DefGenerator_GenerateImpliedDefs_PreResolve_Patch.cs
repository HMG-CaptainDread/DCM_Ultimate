using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ProgressionAgriculture
{
	[HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
	public static class DefGenerator_GenerateImpliedDefs_PreResolve_Patch
	{
		public static HashSet<ThingDef> sowableCrops = new HashSet<ThingDef>();
		public static HashSet<ThingDef> generatedBundles = new HashSet<ThingDef>();
		public static void Postfix(bool hotReload = false)
		{
			foreach (var faction in DefDatabase<FactionDef>.AllDefs)
			{
				if (faction.techLevel < TechLevel.Spacer && faction.caravanTraderKinds != null)
				{
					faction.caravanTraderKinds.Add(PA_DefOf.AgriculturalTrader);
				}
			}

			var traderKindDefs = DefDatabase<TraderKindDef>.AllDefs.Where(x => x.defName.Contains("Bulk"));
			foreach (var traderKindDef in traderKindDefs.ToList())
			{
				traderKindDef.stockGenerators.Add(new StockGenerator_Tag { tradeTag = "Seeds", thingDefCountRange = new IntRange(2, 4), countRange = new IntRange(1, 1) });
			}

			foreach (ThingDef cropDef in DefDatabase<ThingDef>.AllDefs.Where(x => x.plant != null && x.plant.Sowable).ToList())
			{
				if (sowableCrops.Contains(cropDef) ||
						(ModsConfig.IsActive("kentington.saveourship2")
                        	&& cropDef.plant?.sowResearchPrerequisites?.Any(researchDef => researchDef.defName == "ArchotechPlants") == true)){
					continue;
				}

				sowableCrops.Add(cropDef);
				ThingDef seedBundleDef = GenerateSeedBundleDefForCrop(cropDef, hotReload);
				if (seedBundleDef != null)
				{
					generatedBundles.Add(seedBundleDef);
					var researches = cropDef.plant.sowResearchPrerequisites;
					if (researches != null)
					{
						cropDef.plant.sowResearchPrerequisites = new List<ResearchProjectDef>();
					}
					DefGenerator.AddImpliedDef(seedBundleDef, hotReload);
					if (cropDef.plant.harvestedThingDef != null)
					{
						RecipeDef recipeDef = GenerateRecipeDefForCrop(cropDef, seedBundleDef, doubleCost: false);
						if (recipeDef != null)
						{
							DefGenerator.AddImpliedDef(recipeDef, hotReload);
						}
						RecipeDef recipeDefDouble = GenerateRecipeDefForCrop(cropDef, seedBundleDef, doubleCost: true);
						if (recipeDefDouble != null)
						{
							DefGenerator.AddImpliedDef(recipeDefDouble, hotReload);
						}
					}
				}
			}
		}

		private static RecipeDef GenerateRecipeDefForCrop(ThingDef cropDef, ThingDef seedBundleDef, bool doubleCost)
		{
			RecipeDef recipeDef = new RecipeDef();
			recipeDef.defName = "Make" + cropDef.defName + "SeedBundle";
			if (doubleCost)
			{
				recipeDef.defName += "DoubleCost";
			}
			recipeDef.label = "PA.RecipeLabelMakeSeedBundle".Translate(cropDef.label);
			recipeDef.description = "PA.RecipeDescriptionCraftSeedBundleFrom".Translate(cropDef.plant.harvestedThingDef.label, 10, cropDef.plant.harvestedThingDef.label);
			recipeDef.jobString = "PA.RecipeJobStringPackingSeeds".Translate(cropDef.label);
			recipeDef.workAmount = 100;
			recipeDef.workerClass = typeof(RecipeWorker_Seed);
			recipeDef.ingredients = new List<IngredientCount>
			{
				new IngredientCount
				{
					filter = new ThingFilter
					{
						thingDefs = new List<ThingDef> { cropDef.plant.harvestedThingDef }
					},
					count = ProgressionAgricultureModSettings.RecipeDefIngredientCount
				}
			};

			recipeDef.products = new List<ThingDefCountClass>
			{
				new ThingDefCountClass(seedBundleDef, 1)
			};

			recipeDef.recipeUsers = doubleCost
			? new List<ThingDef>
			{
				PA_DefOf.SeedPackingCraftingSpot,
			} : new List<ThingDef>
			{
				PA_DefOf.SeedPackingBench,
			};

			recipeDef.ingredients = new List<IngredientCount>
						 {
							 new IngredientCount
							 {
								 filter = new ThingFilter
								 {
									 thingDefs = new List<ThingDef> { cropDef.plant.harvestedThingDef }
								 },
								 count = doubleCost ? ProgressionAgricultureModSettings.RecipeDefIngredientCount * 2 : ProgressionAgricultureModSettings.RecipeDefIngredientCount
							 }
						 };
			return recipeDef;
		}

		private static ThingDef GenerateSeedBundleDefForCrop(ThingDef cropDef, bool hotReload)
		{
			ThingDef seedBundleDef = new ThingDef();
			seedBundleDef.defName = cropDef.defName + "_SeedBundle";
			seedBundleDef.label = "PA.SeedBundleLabelSeedBundle".Translate(cropDef.label);
			seedBundleDef.description = "PA.SeedBundleDescriptionBundleOfSeedsForCrop".Translate(cropDef.label);
			seedBundleDef.thingClass = typeof(ThingWithComps);
			seedBundleDef.graphicData = new GraphicData
			{
				texPath = "SeedBundle",
				graphicClass = typeof(Graphic_Single),
				shaderType = ShaderTypeDefOf.Cutout
			};
			seedBundleDef.thingCategories = new List<ThingCategoryDef> { PA_DefOf.SeedBundles };
			seedBundleDef.stackLimit = 1;
			seedBundleDef.tradeability = Tradeability.All;
			seedBundleDef.tradeTags = new List<string> { "Seeds" };
			seedBundleDef.selectable = true;
			seedBundleDef.thingSetMakerTags = new List<string>
			{
				"RewardStandardCore"
			};
			seedBundleDef.descriptionHyperlinks = new List<DefHyperlink>
				{
					new DefHyperlink(cropDef)
				};
			seedBundleDef.useHitPoints = false;
			seedBundleDef.alwaysHaulable = true;
			seedBundleDef.category = ThingCategory.Item;
			seedBundleDef.altitudeLayer = AltitudeLayer.Item;
			seedBundleDef.tickerType = TickerType.Never;
			seedBundleDef.rotatable = false;
			seedBundleDef.pathCost = 14;
			seedBundleDef.drawGUIOverlay = true;
			seedBundleDef.statBases =
			[
				new StatModifier { stat = StatDefOf.MaxHitPoints, value = 100 },
				new StatModifier { stat = StatDefOf.MarketValue, value = 300 },
				new StatModifier { stat = StatDefOf.Mass, value = 0.02f },
				new StatModifier { stat = StatDefOf.SellPriceFactor, value = 0.04f }
			];
			seedBundleDef.techLevel = TechLevel.Neolithic;
			seedBundleDef.scatterableOnMapGen = false;
			seedBundleDef.resourceReadoutPriority = ResourceCountPriority.Middle;
			seedBundleDef.comps.Add(new CompProperties_UnlockCrop { cropDef = cropDef });
			seedBundleDef.comps.Add(new CompProperties_Forbiddable());
			return seedBundleDef;
		}
	}
}