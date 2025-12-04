using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public abstract class TimeModifier
    {
        private string patchTag;

        protected virtual int ModifyTime(IntVec3 center, Map map, int timeInput)
        {
            return timeInput;
        }

        public int Modify(IntVec3 center, Map map, int timeInput)
        {
            return ModifyTime(center, map, timeInput);
        }

        public int Modify(int center, Map map, int timeInput)
        {
            return ModifyTime(Verse.CellIndicesUtility.IndexToCell(center, map.Size.x), map, timeInput);
        }
    }
}
