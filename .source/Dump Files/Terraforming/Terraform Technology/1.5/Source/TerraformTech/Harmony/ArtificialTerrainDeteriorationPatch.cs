using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TerraformTech
{

    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
    class ArtificialTerrainDeteriorationPatch
    {
        [HarmonyPostfix]
        static void Postfix()
        {
            foreach (var terrainDef in DefDatabase<TerrainDef>.AllDefs)
            {
                //Log.Message(string.Format("[terrain] {0}", terrainDef.defName));
                //Log.Message(string.Format("[MadeFromStuff] {0}", terrainDef.MadeFromStuff));
                //Log.Message(string.Format("[DeteriorationRate] {0}", StatExtension.StatBaseDefined(terrainDef, StatDefOf.DeteriorationRate)));
                if (!terrainDef.natural && !terrainDef.MadeFromStuff && !StatExtension.StatBaseDefined(terrainDef, StatDefOf.DeteriorationRate))
                {
                    float deterioration = TerraformHelpers.GetTotalDefDeterioration(terrainDef);

                    StatExtension.SetStatBaseValue(terrainDef, StatDefOf.DeteriorationRate, deterioration);

                   // Log.Message(string.Format("terrain {0} deterioration: {1}, is natural {2}", terrainDef.defName, deterioration, terrainDef.natural));
                }
            }
        }
    }
}
