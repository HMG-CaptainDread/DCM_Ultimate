using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class PlantDefChecker : CheckerBase
    {
        protected List<ThingDef> defs;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            Plant plant = center.GetPlant(map);
            if (plant != null)
            {
                if(defs != null && defs.Count > 0)
                if(defs.Contains(plant.def))
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
