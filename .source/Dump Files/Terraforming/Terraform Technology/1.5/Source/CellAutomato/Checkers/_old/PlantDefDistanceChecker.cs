using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    //test whether a cell in range has all of tags in mustinclude and no tags with mustnotinclude
    public class PlantDefDistanceChecker : CheckerBase
    {
        public float range;
        public Success mode;
        public List<ThingDef> defs;


        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            int num = GenRadial.NumCellsInRadius(range);
            IntVec3 curCenter;
            Plant plant;

            //Log.Message("TerrainDistanceChecker");
            if (defs != null && defs.Count > 0)
            {
                if(mode == Success.Normal)
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);

                        if (curCenter.InBounds(map))
                        {
                            plant = GridsUtility.GetPlant(curCenter, map);

                            if (plant != null && defs.Contains(plant.def))
                            {
                                return success == Success.Normal ? true : false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);

                        if (curCenter.InBounds(map))
                        {
                            plant = GridsUtility.GetPlant(curCenter, map);

                            if (plant != null && !defs.Contains(plant.def))
                            {
                                return success == Success.Normal ? true : false;
                            }
                        }
                    }
                }
            }
            else
            {
                if (mode == Success.Normal)
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);

                        if (curCenter.InBounds(map))
                        {
                            plant = GridsUtility.GetPlant(curCenter, map);

                            if (plant != null)
                            {
                                return success == Success.Normal ? true : false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < num; ++i)
                    {
                        curCenter = (center + GenRadial.RadialPattern[i]);

                        if (curCenter.InBounds(map))
                        {
                            plant = GridsUtility.GetPlant(curCenter, map);

                            if (plant != null)
                            {
                                return success == Success.Normal ? true : false;
                            }
                        }
                    }
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
