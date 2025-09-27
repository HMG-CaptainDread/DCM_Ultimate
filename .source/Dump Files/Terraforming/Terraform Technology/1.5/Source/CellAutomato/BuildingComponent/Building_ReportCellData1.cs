using Verse;

namespace CellAutomato
{
    public class TestDataCompProperties1 : CompProperties
    {
        public TileInfoChecker checker;
        
        public TestDataCompProperties1()
        {
            compClass = typeof(TestDataComp1);
        }
    }

    public class TestDataComp1 : ThingComp
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

            float result = ((TestDataCompProperties1)props).checker.CheckResult(center, map);
            bool result2 = ((TestDataCompProperties1)props).checker.Check(center, map, true);
            

            return "result:" + result +
                "\n check result: " + result2 +
                "\n swampiness: " + map.TileInfo.swampiness +
                "\n humidity:" + map.TileInfo.rainfall +
                "\n wind: " + windSpeed +
                "\n rain: " + RainRate +
                "\n temp: " + Temperature;
        }

    }
}
