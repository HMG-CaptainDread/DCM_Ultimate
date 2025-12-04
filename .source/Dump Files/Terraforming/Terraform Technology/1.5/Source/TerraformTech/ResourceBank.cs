using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TerraformTech
{
    public static class ResourceBank
    {
        public static Dictionary<string, TerrainDef> NaturalStoneTerrains = new Dictionary<string, TerrainDef>();
        
        private static Dictionary<string, ThingDef> naturalRocTokWalls = null;
        
        public static Dictionary<string, ThingDef> NaturalRockToWalls
        {
            get
            {
                if (naturalRocTokWalls == null)
                {
                    naturalRocTokWalls = DefDatabase<ThingDef>.AllDefs.Where(def => def.building?.isNaturalRock == true && def.building?.mineableThing != null).ToDictionary(item => item.building.mineableThing.defName);
                }
                return naturalRocTokWalls;
            }
        }

        [DefOf]
        public static class DesignationDefOf
        {
            public static DesignationDef Designation_ReconstructNaturalWall;
        }
        

        [DefOf]
        public static class JobDefOf
        {
            public static JobDef ReconstructNaturalWall;
        }

        [StaticConstructorOnStartup]
        public static class Textures
        {
            public static Texture2D ReconstructNaturalWall = ContentFinder<Texture2D>.Get("UI/Designators/ReconstructWall");
        }

        public class TerraformSettings
        {
            public class Reconstruct
            {
                public const float WarmupTicks = 80f;

                public const float TicksBetweenRepairs = 20f;
            }
        }

        public static class Strings
        {      
            public static string CommandReconstructNatural = "CommandReconstructNatural";
            public static string CommandReconstructNaturalDesc = "CommandReconstructNaturalDesc";
        }
    }
}