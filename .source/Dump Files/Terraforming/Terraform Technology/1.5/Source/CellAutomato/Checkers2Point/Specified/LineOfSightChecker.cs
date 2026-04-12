using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class LineOfSightChecker : Check2PointNode
    {
        public override bool Check(IntVec3 start, IntVec3 end, Map map, bool secondCheck = false, bool debug = false)
        {
            if(GenSight.LineOfSight(start, end, map))
            {
                return success == Success.Normal ? true : false;
            }
            else
            {
                return success == Success.Normal ? false : true;
            }
        }
       
    }
}
