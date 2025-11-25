using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class AdjancentRoofedChecker : CheckerBase
    {
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            IntVec3 curCenter;
            foreach(IntVec3 vec in GenAdj.AdjacentCells)
            {
                curCenter = center + vec;
                if (curCenter.InBounds(map) && map.roofGrid.Roofed(curCenter))
                {
                    return success == Success.Normal ? true : false;
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
