using RimWorld;
using System.Reflection;
using Verse;

namespace CellAutomato
{
    public class SpawnPlantAction : RuleAction
    {
        ThingDef plantDef = null;
        float growthInitial = 0.001f;
        
        protected override void ApplyRule(Verse.IntVec3 center, Map map)
        {
            if (chance < 1f)
                if (chance > 0 && Rand.Chance(chance)) { }
                else return;

            var plant = GenSpawn.Spawn(plantDef, center, map);
            if(plant != null)
            {
                if(plant is Plant p)
                {
                    p.Growth = growthInitial;
                    p.sown = false;
                    map.mapDrawer.MapMeshDirty(center, MapMeshFlagDefOf.Things);
                }
                else
                {
                    plant.Destroy();
                }
            }
        }
    }
}
