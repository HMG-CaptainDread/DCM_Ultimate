using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace ProgressionAgriculture
{
    public class ScenPart_StartingSeeds : ScenPart
    {
        public ThingDef thingDef;
        public const string PlayerStartWithTag = "PA.PlayerStartingSeeds";
        public static string PlayerStartWithIntro => "PA.ScenPart_StartingSeeds".Translate();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref thingDef, "thingDef");
        }
        
        public override string Summary(Scenario scen)
        {
            return ScenSummaryList.SummaryWithList(scen, "PA.PlayerStartingSeeds", PlayerStartWithIntro);
        }

        public override IEnumerable<string> GetSummaryListEntries(string tag)
        {
            if (tag == "PA.PlayerStartingSeeds")
            {
                yield return GenLabel.ThingLabel(thingDef, null).CapitalizeFirst();
            }
        }

        public override void Randomize()
        {
            thingDef = PossibleThingDefs().RandomElement();
        }

        public IEnumerable<ThingDef> PossibleThingDefs()
        {
            return DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.thingCategories!= null && d.thingCategories.Contains(PA_DefOf.SeedBundles));
        }

        public override IEnumerable<Thing> PlayerStartingThings()
        {
            Thing thing = ThingMaker.MakeThing(thingDef);
            yield return thing;
        }

        public override void DoEditInterface(Listing_ScenEdit listing)
        {
            Rect scenPartRect = listing.GetScenPartRect(this, RowHeight * 4f);
            Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 4f);
            if (Widgets.ButtonText(rect, thingDef.LabelCap, drawBackground: true, doMouseoverSound: true, active: true, null))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (ThingDef item in from t in PossibleThingDefs()
                    orderby t.label
                    select t)
                {
                    ThingDef localTd = item;
                    list.Add(new FloatMenuOption(localTd.LabelCap, delegate
                    {
                        thingDef = localTd;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }
        }
    }
}
