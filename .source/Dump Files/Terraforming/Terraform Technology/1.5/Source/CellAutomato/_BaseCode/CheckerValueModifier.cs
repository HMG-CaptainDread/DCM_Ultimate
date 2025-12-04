using Verse;

namespace CellAutomato
{
    public abstract class CheckerValueModifier
    {
        public virtual float Modify(IntVec3 center, Map map, float value)
        {
            return value;
        }
    }

    enum ValueFactorType
    {
        Multiply,
        Add
    }

    public class CheckerValueFactor
    {
        ValueFactorType type;
        Verse.SimpleCurve factorCurve;

        public virtual float Factor(float sourceValue, float factorValue)
        {
            if(factorCurve != null)
            {
                if (type == ValueFactorType.Multiply)
                {
                    return sourceValue * factorCurve.Evaluate(factorValue);
                }
                else
                if (type == ValueFactorType.Add)
                {
                    return sourceValue + factorCurve.Evaluate(factorValue);
                }
            }
            
            return sourceValue;
        }
    }
}
