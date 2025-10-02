using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public enum CheckerType
    {
        And,
        Or
    }

    public enum Success
    {
        Normal,
        Invert
    }

    public enum Mode
    {
        Single,
        All
    }

    public enum DistanceSelection
    {
        Closest,
        Furthest
    }
}
