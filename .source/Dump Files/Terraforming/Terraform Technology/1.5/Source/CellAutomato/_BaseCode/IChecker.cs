using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public interface IChecker
    {
        bool Check(int center, Map map, bool secondCheck = false, bool debug = false);
    }
}
