using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class SunExposedChecker : CheckerBase
    {
        private float minGlow = 0.51f;  

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if(map.roofGrid.RoofAt(center) == null)
            {
                return success == Success.Normal ? true : false;
            }
            else
            if(map.glowGrid.GroundGlowAt(center, true) >= minGlow)
            {
                return success == Success.Normal ? true : false;
            }

            return success == Success.Normal ? false : true;
        }
    }
}
