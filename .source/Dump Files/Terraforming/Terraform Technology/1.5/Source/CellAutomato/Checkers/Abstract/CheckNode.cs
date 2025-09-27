using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class CheckNode : CheckerBase
    {
        public CheckerType operation = CheckerType.And;
        public List<CheckerBase> subNodes;
        
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            if (subNodes != null)
            {
                if (operation == CheckerType.And)
                {
                    bool result = true;
                    foreach (var rule in subNodes)
                        if (result)
                        {
                            result &= rule.Check(center, map, secondCheck, debug);
                            if (debug)
                            {
                                Log.Message("Rule " + rule.GetType().Name + " result: " + result);
                            }
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
                        result |= rule.Check(center, map, secondCheck, debug);
                        if (debug)
                        {
                            Log.Message("Rule " + rule.GetType().Name + " result: " + result);
                        }
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
