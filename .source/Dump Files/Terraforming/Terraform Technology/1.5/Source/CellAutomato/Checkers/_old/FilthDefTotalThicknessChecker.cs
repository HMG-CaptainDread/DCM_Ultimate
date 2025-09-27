using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    //test whether a cell in range has all of tags in mustinclude and no tags with mustnotinclude
    public class FilthDefTotalThicknessChecker : CheckerBase
    {
        public IntRange range;
        public Success mode;
        public List<ThingDef> defs;
        
        public override bool Check(IntVec3 curCenter, Map map, bool secondCheck = false, bool debug = false)
        {
            if (defs != null && defs.Count > 0)
            {
                int count = 0;
                if (curCenter.InBounds(map))
                {
                    List<Thing> thingList = map.thingGrid.ThingsListAtFast(curCenter);
                    foreach (var thing in thingList)
                    {
                        if (thing.def.category == ThingCategory.Filth && defs.Contains(thing.def))
                        {
                            if(thing is Filth f && f.thickness > 0)
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
                
                if(range.min <= count && count <= range.max)
                {
                    return success == Success.Normal ? true : false;
                }
            }
            else
            {
                int count = 0;
                if (curCenter.InBounds(map))
                {
                    List<Thing> thingList = map.thingGrid.ThingsListAtFast(curCenter);
                    foreach (var thing in thingList)
                    {
                        if (thing.def.category == ThingCategory.Filth)
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

                if (range.min <= count && count <= range.max)
                {
                    return success == Success.Normal ? true : false;
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
