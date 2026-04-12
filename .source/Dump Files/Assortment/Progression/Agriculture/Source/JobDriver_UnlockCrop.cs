using Verse;
using Verse.AI;

namespace ProgressionAgriculture
{
	public class JobDriver_UnlockCrop : JobDriver
	{
		public override string GetReport()
		{
			return job.def.reportString.Formatted(Comp().Props.cropDef.label);

		}

		private CompUnlockCrop Comp()
		{
			return TargetA.Thing.TryGetComp<CompUnlockCrop>();
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(TargetA, job, 1, -1, null, errorOnFailed);
		}

		public override System.Collections.Generic.IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_General.Wait(60).WithProgressBarToilDelay(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				CompUnlockCrop comp = Comp();
				if (comp != null)
				{
					comp.Unlock();
				}
			});
		}
	}
}