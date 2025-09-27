using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TerraformTech
{
    public static class TerraformHelpers
    {
        static Dictionary<string, float> massDict = new Dictionary<string, float>();

        public static float GetTotalDefDeterioration(BuildableDef def)
        {
            massDict.Clear();

            float totalMass = 0, mass, retDeterioration = 0.0f;
            if(def.costList != null)
            {
                foreach (var costItem in def.costList)
                {
                    mass = costItem.count * costItem.thingDef.BaseMass;
                    totalMass += mass;
                    massDict[costItem.thingDef.defName] = mass;
                }

                float currDeterioration;

                foreach (var costItem in def.costList)
                {
                    currDeterioration = 1.0f;
                    if (StatExtension.StatBaseDefined(costItem.thingDef, StatDefOf.DeteriorationRate))
                    {
                        currDeterioration = StatExtension.GetStatValueAbstract(costItem.thingDef, StatDefOf.DeteriorationRate);
                    }

                    if (costItem.thingDef.IsStuff && costItem.thingDef.stuffProps.statFactors != null)
                    {
                        currDeterioration *=
                            costItem.thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.DeteriorationRate)
                            /
                            costItem.thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints);
                    }

                    retDeterioration = (massDict[costItem.thingDef.defName] / totalMass) * currDeterioration;
                }
            }
            
            return retDeterioration;
            //return 1;
        }

        public static float GetTotalThingDeterioration(Thing thing)
        {
            if (StatExtension.StatBaseDefined(thing.def, StatDefOf.DeteriorationRate))
            {
                return StatExtension.GetStatValueAbstract(thing.def, StatDefOf.DeteriorationRate);
            }

            massDict.Clear();
            float totalMass = 0, mass;
            foreach (var costItem in thing.def.costList)
            {
                mass = costItem.count * costItem.thingDef.BaseMass;
                totalMass += mass;
                massDict[costItem.thingDef.defName] = mass;
            }

            if (thing.def.MadeFromStuff)
            {
                ThingDef stuff = thing.Stuff;

                totalMass += thing.def.costStuffCount * stuff.GetStatValueAbstract(StatDefOf.Mass);
            }

            float
                retDeterioration = 0.0f,
                currDeterioration;

            foreach (var costItem in thing.def.costList)
            {
                currDeterioration = 1.0f;
                if (StatExtension.StatBaseDefined(costItem.thingDef, StatDefOf.DeteriorationRate))
                {
                    currDeterioration = StatExtension.GetStatValueAbstract(costItem.thingDef, StatDefOf.DeteriorationRate);
                }

                if (costItem.thingDef.IsStuff && costItem.thingDef.stuffProps.statFactors != null)
                {
                    currDeterioration *=
                        costItem.thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.DeteriorationRate)
                        /
                        costItem.thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints);
                }

                retDeterioration += (massDict[costItem.thingDef.defName] / totalMass) * currDeterioration;
            }

            if (thing.def.MadeFromStuff)
            {
                ThingDef stuffDef = thing.Stuff;

                currDeterioration = 1.0f;
                if (StatExtension.StatBaseDefined(stuffDef, StatDefOf.DeteriorationRate))
                {
                    currDeterioration = StatExtension.GetStatValueAbstract(stuffDef, StatDefOf.DeteriorationRate);
                }

                if (stuffDef.IsStuff && stuffDef.stuffProps.statFactors != null)
                {
                    currDeterioration *=
                        stuffDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.DeteriorationRate)
                        /
                        stuffDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints);
                }

                retDeterioration += ((thing.def.costStuffCount * stuffDef.GetStatValueAbstract(StatDefOf.Mass)) / totalMass) * currDeterioration;
            }


            return retDeterioration;
        }
    }
}