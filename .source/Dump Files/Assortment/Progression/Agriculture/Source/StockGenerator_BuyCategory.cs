using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ProgressionAgriculture
{
public class StockGenerator_BuyCategory : StockGenerator
    {
        private ThingCategoryDef categoryDef;

        private IntRange thingDefCountRange = IntRange.one;

        private List<ThingDef> excludedThingDefs;

        private List<ThingCategoryDef> excludedCategories;
        public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
        {
            return Enumerable.Empty<Thing>();
        }

        public override bool HandlesThingDef(ThingDef t)
        {
            if (categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != 0 && (int)t.techLevel <= (int)maxTechLevelBuy && (excludedThingDefs == null || !excludedThingDefs.Contains(t)))
            {
                if (excludedCategories != null)
                {
                    return !excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t));
                }
                return true;
            }
            return false;
        }
    }
}
