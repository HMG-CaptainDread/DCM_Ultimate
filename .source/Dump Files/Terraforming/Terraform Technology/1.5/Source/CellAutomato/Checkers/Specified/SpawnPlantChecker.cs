using RimWorld;
using Verse;

namespace CellAutomato
{
    public class SpawnPlantChecker : CheckerBase
    {
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (center.GetPlant(map) != null || 
                center.GetCover(map) != null || 
                center.GetEdifice(map) != null || 
                !PlantUtility.SnowAllowsPlanting(center, map))
            {
                return success == Success.Normal ? false : true;
            }

            return success == Success.Normal ? true : false;
        }
    }
}
