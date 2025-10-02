using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    public class FilthDefChecker : CheckerBase
    {
        public Mode mode;
        public List<ThingDef> defs;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (defs != null && defs.Count > 0)
            {
                List<Thing> thingList = map.thingGrid.ThingsListAtFast(center);
                foreach (var thing in thingList)
                {
                    if (thing.def.category == ThingCategory.Filth)
                    if (defs.Contains(thing.def))
                    {
                        if (mode == Mode.Single)
                        {
                            return success == Success.Normal ? true : false;
                        }
                    }
                    else
                    {
                        if (mode == Mode.All)
                        {
                            return success == Success.Normal ? false : true;
                        }
                    }
                }
            }

            return success == Success.Normal ? false : true;
        }

        protected override float GetValue(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            float count = 0;

            if(defs != null && defs.Count > 0)
                if (center.InBounds(map))
                {
                    List<Thing> thingList = map.thingGrid.ThingsListAtFast(center);
                    foreach (var thing in thingList)
                    {
                        if (thing.def.category == ThingCategory.Filth && defs.Contains(thing.def))
                        {
                            if (thing is Filth f && f.thickness > 0)
                            {
                                count += f.thickness;
                            }
                            else
                            {
                                ++count;
                            }
                        }
                    }
                }

            return count;
        }
    }
}
