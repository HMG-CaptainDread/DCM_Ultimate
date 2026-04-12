using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public abstract class Checker2PointBase
    {
        public string patchTag;
        public Success success;

        public bool Check(int start, int end, Map map, bool secondCheck = false)
        {
            return Check(map.cellIndices.IndexToCell(start), map.cellIndices.IndexToCell(end), map);
        }

        public virtual bool Check(IntVec3 start, IntVec3 end, Map map, bool secondCheck = false, bool debug = false)
        {
            return true;
        }
    }
}
