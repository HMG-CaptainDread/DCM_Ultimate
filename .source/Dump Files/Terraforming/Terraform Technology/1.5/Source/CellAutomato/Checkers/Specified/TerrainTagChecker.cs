using System.Collections.Generic;
using TerraformTech;
using Verse;

namespace CellAutomato
{
    //test whether a cell in range has all of tags in mustinclude and no tags with mustnotinclude
    public class TerrainTagChecker : CheckerBase
    {
        public Mode mode;
        public List<string> tags;

        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (tags != null && tags.Count > 0)
            {
                var terrain = TerraformHelper.GetTerrain(map, center);
                if (terrain.tags != null && terrain.tags.Count > 0)
                {
                    foreach (var tag in tags)
                    {
                        if (terrain.tags.Contains(tag))
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
            }
            else
            {
                if (mode == Mode.Single)
                {
                    return success == Success.Normal ? true : false;
                }

                var terrain = TerraformHelper.GetTerrain(map, center);

                //mode = Mode.All
                if (terrain.tags != null && terrain.tags.Count > 0)
                {
                    return success == Success.Normal ? true : false;
                }
                else
                {
                    return success == Success.Normal ? false : true;
                }
            }

            return success == Success.Normal ? false : true;
        }
    }
}
