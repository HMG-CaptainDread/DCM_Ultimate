using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using static TerraformTech.ResourceBank;

namespace TerraformTech
{
    public class JobDriver_ReconstructNaturalWall : JobDriver
    {
        private Effecter effecter;

        protected float ticksToNextRepair;

        private Thing RepairTarget => job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(delegate
            {
                Designation designation = Map.designationManager.DesignationOn(TargetThingA, ResourceBank.DesignationDefOf.Designation_ReconstructNaturalWall);
                if (designation != null)
                {
                    return false;
                }
                return true;
            });
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Toil repair = new Toil();
            repair.initAction = delegate
            {
                ticksToNextRepair = TerraformSettings.Reconstruct.WarmupTicks;
            };
            repair.tickAction = delegate
            {
                Thing repairTarget = RepairTarget;
                Pawn actor = repair.actor;
                actor.skills.Learn(SkillDefOf.Construction, 0.05f);
                float num = actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f;
                ticksToNextRepair -= num;
                if (ticksToNextRepair <= 0f)
                {
                    if (effecter == null)
                    {
                        effecter = EffecterDefOf.Mine.Spawn();
                    }
                    effecter.Trigger(actor, repairTarget);

                    ticksToNextRepair += TerraformSettings.Reconstruct.TicksBetweenRepairs;
                    base.TargetThingA.HitPoints++;
                    base.TargetThingA.HitPoints = Mathf.Min(base.TargetThingA.HitPoints, base.TargetThingA.MaxHitPoints);
                    base.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)base.TargetThingA);
                    if (base.TargetThingA.HitPoints == base.TargetThingA.MaxHitPoints)
                    {
                        actor.records.Increment(RecordDefOf.ThingsRepaired);
                        actor.jobs.EndCurrentJob(JobCondition.Succeeded);
                    }
                }
            };
            repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            repair.defaultCompleteMode = ToilCompleteMode.Never;
            repair.activeSkill = (() => SkillDefOf.Construction);

            yield return repair;
        }
    }
}      