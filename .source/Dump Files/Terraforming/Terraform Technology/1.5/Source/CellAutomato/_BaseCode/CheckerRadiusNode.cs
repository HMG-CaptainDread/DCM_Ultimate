using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public abstract class CheckerRadiusNode : CheckerBase
    {
        protected float range;
        protected CheckerBase checker;
        protected Check2PointNode checker2Point;
    }   
}
