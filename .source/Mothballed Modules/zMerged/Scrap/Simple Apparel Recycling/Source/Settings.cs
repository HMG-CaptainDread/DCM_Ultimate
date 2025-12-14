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
    public class Settings : ModSettings
    {
        static public float efficiency = 0.25f; // default smelt efficiency is 0.25f
        static public bool isDurabilityInfluenced = false; 

        public override void ExposeData()
        {
            Scribe_Values.Look(ref efficiency, "efficiency", 0.25f, true);
            Scribe_Values.Look(ref isDurabilityInfluenced, "isDurabilityInfluenced", true, true);
            base.ExposeData();
        }

    }

}