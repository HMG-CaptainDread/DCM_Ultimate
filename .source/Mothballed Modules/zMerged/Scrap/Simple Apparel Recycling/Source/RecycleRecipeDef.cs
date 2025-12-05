using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Verse.Noise;
using Verse.Grammar;
using RimWorld;
using RimWorld.Planet;

using System.Reflection;
using HarmonyLib;

namespace AVSR
{
    /*
        All recipes are generated and resolved during game load time
        `ResolveReferences()` is called during that time
    */
    public class RecycleRecipeDef : RecipeDef
    {
        public override void ResolveReferences()
        {
            string[] forceIncludeDefs = new string[] {"Apparel_Cape"};
            
            FieldInfo fieldInfo = typeof(ThingFilter).GetField("thingDefs", BindingFlags.NonPublic | BindingFlags.Instance);
            List<ThingDef> xmlDefsList = (List<ThingDef>)fieldInfo.GetValue(fixedIngredientFilter);
            List<ThingDef> neolithicDefsList = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => 
                x.techLevel == TechLevel.Neolithic && 
                x.thingClass == typeof(Apparel) ||
                forceIncludeDefs.Contains(x.defName) 
            ).ToList();

            fieldInfo.SetValue(fixedIngredientFilter, neolithicDefsList.Concat(xmlDefsList).ToList());
            base.ResolveReferences();
        }

    }
}