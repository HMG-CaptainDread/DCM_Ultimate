using Verse;

namespace TerraformTech
{
    public class Building_TerraformStoneWall : Building
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            string thingdefName = this.Stuff.defName;

            if (ResourceBank.NaturalRockToWalls.ContainsKey(thingdefName))
            {
                ThingDef RockToSpawn = ResourceBank.NaturalRockToWalls[thingdefName];

                this.Destroy(DestroyMode.Vanish);

                GenSpawn.Spawn(RockToSpawn, base.Position, map, WipeMode.VanishOrMoveAside);
            }
            else
            {
                this.Destroy(DestroyMode.FailConstruction);
            } 
        }
    }
}
