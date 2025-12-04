using Verse;

namespace CellAutomato
{
    public class TestDataCompProperties : CompProperties
    {
        public SwampinessChecker swampinessChecker;
        
        public TestDataCompProperties()
        {
            compClass = typeof(TestDataComp);
        }
    }

    public class TestDataComp : ThingComp
    {
        public override string CompInspectStringExtra()
        {
            var map = parent.Map;
            var center = parent.Position;
            float windSpeed = 0;
            float RainRate = 0;
            float Temperature = 0;

            var room = parent.Position.GetRoom(map);
            if(room == null || (room != null && !room.ProperRoom))
            {
                windSpeed = parent.Map.windManager.WindSpeed;
            }
            else
            {
                windSpeed = 0;
            }

            if (!map.roofGrid.Roofed(center))
            {
                RainRate = map.weatherManager.RainRate;
            }

            if (room != null)
            {
                Temperature = room.UsesOutdoorTemperature ? map.mapTemperature.OutdoorTemp : room.Temperature;
            }
            else
            {
                Temperature = map.mapTemperature.OutdoorTemp;
            }

            var result = ((TestDataCompProperties)props).swampinessChecker.CheckResult(center, map);

            return "result:" + result +
                "\n swampiness: " + map.TileInfo.swampiness +
                "\n humidity:" + map.TileInfo.rainfall +
                "\n wind: " + windSpeed +
                "\n rain: " + RainRate +
                "\n temp: " + Temperature;
        }

    }
}
