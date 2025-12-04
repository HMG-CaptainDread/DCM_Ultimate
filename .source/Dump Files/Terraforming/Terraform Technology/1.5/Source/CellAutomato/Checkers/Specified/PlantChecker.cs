using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class PlantChecker : CheckerBase
    {
        protected List<PlantPurpose> plantPurpose;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            Plant plant = center.GetPlant(map);
            if (plant != null)
            {
                if(plantPurpose != null && plantPurpose.Count > 0)
                if(plantPurpose.Any(purp => plant.def.plant.purpose == purp))
                {
                    return success == Success.Normal ? true : false;
                }
                else
                {
                    return success == Success.Normal ? false : true;
                }
                    
                return success == Success.Normal ? true : false;
            }

            return success == Success.Normal ? false : true;
        }
    }
}
