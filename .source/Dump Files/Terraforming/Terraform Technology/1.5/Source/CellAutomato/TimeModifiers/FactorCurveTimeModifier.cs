using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class FactorCurveTimeModifier : TimeModifier
    {
        Verse.SimpleCurve factorCurve;
        CheckerBase value;

        protected override int ModifyTime(IntVec3 center, Map map, int timeInput)
        {
            if (value != null && factorCurve != null)
            {
                return (int)(timeInput * factorCurve.Evaluate(value.Get(center, map)));
            }

            return timeInput;
        }
    }
}
