using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class CheckerBase : IValueGetter, IChecker
    {
        public string patchTag;
        public Success success = Success.Normal;

        public bool Check(int center, Map map, bool secondCheck = false, bool debug = false)
        {
            return Check(map.cellIndices.IndexToCell(center), map, secondCheck, debug);
        }

        public float Get(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            return GetValue(center, map);
        }

        public float Get(int center, Map map, bool secondCheck = false, bool debug = false)
        {
            return GetValue(Verse.CellIndicesUtility.IndexToCell(center, map.Size.x), map);
        }

        public virtual bool Check(Verse.IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            return true;
        }

        protected virtual float GetValue(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            return Check(center, map, secondCheck, debug) ? 1 : 0;
        }
    }
}
