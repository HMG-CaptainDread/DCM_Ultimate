using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CellAutomato
{
    public class RoomChecker : CheckerBase
    {
        public override bool Check(IntVec3 center, Map map, bool secondCheck = false, bool debug = false)
        {
            if (!center.InBounds(map))
                return false;

            Room room = GridsUtility.GetRoom(center, map);
            if(room != null && room.ProperRoom && room.Role != RoomRoleDefOf.None)
            {
                if (debug)
                {
                    Log.Message(string.Format("Room cellcount: {0}, is proper room: {1}, room function: {2}",
                        room.CellCount, 
                        room.ProperRoom,
                        room.Role?.defName));
                }

                return success == Success.Normal ? true : false;
            }

            return success == Success.Normal ? false : true;
        }
    }
}
