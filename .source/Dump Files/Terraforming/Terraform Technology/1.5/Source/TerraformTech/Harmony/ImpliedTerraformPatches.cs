using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TerraformTech
{
    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
    class ImpliedTerraformPatches
    {
        [HarmonyPrefix]
        static void Prefix()
        {
            try
            {
                foreach (var terraformTerrainRuleDef in DefDatabase<TerrainTerraformRuleSet>.AllDefs)
                {
                    if (terraformTerrainRuleDef.defaultRuleActionClass == null ||
                        !(typeof(TerraformAction).IsAssignableFrom(terraformTerrainRuleDef.defaultRuleActionClass)) ||
                        terraformTerrainRuleDef.defaultRuleActionClass.IsAbstract)
                    {
                        terraformTerrainRuleDef.defaultRuleActionClass = typeof(TerrainReplaceAction);
                    }
                    
                    //unpack terraform rule
                    foreach (var rule in terraformTerrainRuleDef.rules)
                    {
                        if (rule.ruleActionClass == null)
                        {
                            rule.ruleActionClass = terraformTerrainRuleDef.defaultRuleActionClass;
                        }
                        rule.action = (TerraformAction)Activator.CreateInstance(rule.ruleActionClass);
                        rule.action.preferredResultTerrainDef = rule.resultDef;


                        string defPrefix = terraformTerrainRuleDef.defName + '_',
                               defSuffix = '_' + rule.resultDef.defName;
                        int suffixNumber = 0;

                        while (DefDatabase<ThingDef>.GetNamed(defPrefix + suffixNumber + defSuffix, false) != null)
                        {
                            ++suffixNumber;
                        }

                        TerrainTerraformDef generatedDef = new TerrainTerraformDef();

                        generatedDef.description = terraformTerrainRuleDef.description;
                        generatedDef.drawerType = terraformTerrainRuleDef.drawerType;
                        generatedDef.modContentPack = terraformTerrainRuleDef.modContentPack;
                        generatedDef.altitudeLayer = terraformTerrainRuleDef.altitudeLayer;
                        generatedDef.terrainAffordanceNeeded = terraformTerrainRuleDef.terrainAffordanceNeeded;
                        generatedDef.placingDraggableDimensions = terraformTerrainRuleDef.placingDraggableDimensions;

                        generatedDef.designationCategory = terraformTerrainRuleDef.designationCategory;
                        generatedDef.designatorDropdown = terraformTerrainRuleDef.designatorDropdown;
                        generatedDef.leaveResourcesWhenKilled = terraformTerrainRuleDef.leaveResourcesWhenKilled;
                        generatedDef.category = terraformTerrainRuleDef.category;
                        generatedDef.size = terraformTerrainRuleDef.size;

                        generatedDef.useHitPoints = false;
                        generatedDef.selectable = true;

                        generatedDef.alwaysHaulable = false;
                        generatedDef.rotatable = false;
                        generatedDef.drawGUIOverlay = true;
                        generatedDef.castEdgeShadows = false;
                        generatedDef.socialPropernessMatters = false;

                        generatedDef.tickerType = TickerType.Never;
                        generatedDef.pathCost = DefGenerator.StandardItemPathCost;

                        generatedDef.researchPrerequisites = terraformTerrainRuleDef.researchPrerequisites;


                        if (!(generatedDef.placeWorkers != null))
                            generatedDef.placeWorkers = new List<Type>();

                        if (terraformTerrainRuleDef.placeWorkers != null)
                            generatedDef.placeWorkers.AddRange(terraformTerrainRuleDef.placeWorkers);

                        generatedDef.terraformRule = rule;
                        generatedDef.label = rule.action.GetRuleNameString(rule);
                        generatedDef.defName = defPrefix + suffixNumber + defSuffix;


                        generatedDef.SetStatBaseValue(StatDefOf.WorkToBuild, rule.WorkToBuild);
                        if (!(generatedDef.comps != null))
                        {
                            generatedDef.comps = new List<CompProperties>();
                        }
                        generatedDef.comps.Add(new CompProperties_Forbiddable());

                        generatedDef.costList = new List<ThingDefCountClass>();
                        if (rule.costList != null)
                            generatedDef.costList.AddRange(rule.costList);

                        generatedDef.thingClass = typeof(Building_TerraformTerrain);

                        generatedDef.graphicData = new GraphicData();
                        generatedDef.graphicData.CopyFrom(terraformTerrainRuleDef.graphicData);
                        generatedDef.placeWorkers.Add(typeof(PlaceWorker_Terraforming));


                        //if (generatedDef.graphicData.shaderType == null)
                        //{
                        //    generatedDef.graphicData.shaderType = ShaderTypeDefOf.Cutout;
                        //}
                        //generatedDef.graphic = generatedDef.graphicData.Graphic;
                        //*/

                        rule.terraformDef = generatedDef;

                        generatedDef.generated = true;
                        terraformTerrainRuleDef.modContentPack?.AddDef(generatedDef, "ImpliedDefs");
                        generatedDef.PostLoad();
                        DefDatabase<ThingDef>.Add(generatedDef);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Warning(ex.Message);
            }
        }
    }
}
