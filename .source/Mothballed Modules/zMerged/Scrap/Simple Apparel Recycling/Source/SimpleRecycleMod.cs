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
    class SimpleRecycleMod : Mod
    {
        public SimpleRecycleMod(ModContentPack mcp) : base(mcp)
        {
            GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Recycle efficiency based on apparel durability".Translate(), ref Settings.isDurabilityInfluenced);
            listingStandard.Gap(8);
            listingStandard.Label("Recycle efficiency".Translate() + ": " + Settings.efficiency);
            Settings.efficiency = Mathf.Round(listingStandard.Slider(Settings.efficiency, 0.0f, 1.0f) * 100) / 100.0f;
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Simple Apparel Recycling";
        }
    }
}