using Verse;

namespace CellAutomato
{
    public class WeatherModfier : CheckerValueModifier
    {
        CheckerValueFactor windFactor;
        CheckerValueFactor rainFactor;
        CheckerValueFactor temperatureFactor;

        public override float Modify(IntVec3 center, Map map, float value)
        {

            var room = center.GetRoom(map);
            if(windFactor != null)
            {
                if (room == null || (room != null && !room.ProperRoom))
                {
                    value = windFactor.Factor(value, map.windManager.WindSpeed);
                }
            }

            if (rainFactor != null)
            {
                if (!map.roofGrid.Roofed(center))
                {
                    value = rainFactor.Factor(value, map.weatherManager.RainRate);
                }
            }
            
            if (temperatureFactor != null)
            {
                if(room != null)
                {
                    value = temperatureFactor.Factor(value, room.UsesOutdoorTemperature ? map.mapTemperature.OutdoorTemp : room.Temperature);
                }
                else
                {
                    value = temperatureFactor.Factor(value, map.mapTemperature.OutdoorTemp);
                }
            }

            return value;
        }

    }
}
