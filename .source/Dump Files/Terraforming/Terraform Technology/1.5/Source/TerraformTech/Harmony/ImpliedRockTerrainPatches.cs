using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TerraformTech
{

    [HarmonyPatch(typeof(TerrainDefGenerator_Stone), nameof(TerrainDefGenerator_Stone.ImpliedTerrainDefs))]
    class ImpliedRockTerrainPatches
    {
        [HarmonyPostfix]
        static IEnumerable<TerrainDef> Postfix(IEnumerable<TerrainDef> rockTerrains)
        {
            var newTerrains = new List<TerrainDef>();//this is required due to modifications not saving

            foreach(var terrain in rockTerrains)
            {
                terrain.natural = true;
                ResourceBank.NaturalStoneTerrains[terrain.defName] = terrain;
                newTerrains.Add(terrain);
                //Log.Message(string.Format("terrain {0} is natural: {1}", terrain.defName, terrain.natural));
            }

            return newTerrains;
        }
    }
}
