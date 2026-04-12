using RimWorld;   
using System.Collections.Generic;
using System.Linq;     
using Verse;
using Verse.AI;
   
namespace TerraformTech
{
    public class WorkGiver_ReconstructNaturalWall : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            List<Designation> desList = pawn.Map.designationManager.AllDesignations;
            for (int i = 0; i < desList.Count; i++)
            {
                if (desList[i].def == ResourceBank.DesignationDefOf.Designation_ReconstructNaturalWall)
                {
                    yield return desList[i].target.Thing;
                }
            }  
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (pawn.Map.designationManager.DesignationOn(t, ResourceBank.DesignationDefOf.Designation_ReconstructNaturalWall) == null)
            {
                return false;
            }

            if (t.HitPoints >= t.MaxHitPoints)
            {
                return false;
            }

            LocalTargetInfo target = t;
            bool ignoreOtherReservations = forced;
            if (!pawn.CanReserve(target, 1, -1, null, ignoreOtherReservations))
            {
                return false;
            }
            return true;   
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(ResourceBank.JobDefOf.ReconstructNaturalWall, t);
        }

    }
}
