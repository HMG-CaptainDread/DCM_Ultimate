using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public interface IValueGetter
    {
        float Get(IntVec3 center, Map map, bool secondCheck = false, bool debug = false);

        float Get(int center, Map map, bool secondCheck = false, bool debug = false);
    }
}
