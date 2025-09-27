using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class Check2PointNode : Checker2PointBase
    {
        public CheckerType operation;
        public List<Check2PointNode> subNodes;

        public override bool Check(IntVec3 start, IntVec3 end, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!end.InBounds(map))
                return false;

            if (subNodes != null)
            {
                if (operation == CheckerType.And)
                {
                    bool result = true;
                    foreach (var rule in subNodes)
                        if (result)
                        {
                            result &= rule.Check(start, end, map, secondCheck);
                        }
                        else return success == Success.Normal ? false : true;

                    return success == Success.Normal ? result : !result;
                }
                else
                if (operation == CheckerType.Or)
                {
                    bool result = false;
                    foreach (var rule in subNodes)
                    {
                        result |= rule.Check(start, end, map, secondCheck);
                        if (result) return success == Success.Normal ? true : false;
                    }

                    return success == Success.Normal ? false : true;
                }

                return success == Success.Normal ? false : true;
            }
            else
            {
                return success == Success.Normal ? true : false;
            }
        }
    }
}
