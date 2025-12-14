using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class TileInfoChecker : CheckerBase
    {
        private IntRange valueRange;

        private float swampinessMult = 0f;
        private float pollutionMult = 0f;
        private float avgTempMult = 0f;
        private float elevationMult = 0f;
        private float rainfallMult = 0f;

        private List<CheckerValueModifier> valueModifiers;
        
        public float CheckResult(IntVec3 center, Map map)
        {
            float value =
                swampinessMult * map.TileInfo.swampiness +
                pollutionMult * map.TileInfo.pollution +
                avgTempMult * map.TileInfo.temperature +
                elevationMult * map.TileInfo.elevation +
                rainfallMult * map.TileInfo.rainfall;

            if(valueModifiers != null)
                foreach (var valueModifier in valueModifiers)
                {
                    value = valueModifier.Modify(center, map, value);

                }

            return value;
        }

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            //Log.Message("TileInfoChecker");
            float value = CheckResult(center, map);

            if (valueRange.min < value && value < valueRange.max)
                return success == Success.Normal ? true : false;

            return success == Success.Normal ? false : true;
        }
    }
}
