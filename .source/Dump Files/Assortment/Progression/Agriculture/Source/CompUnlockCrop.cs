using Verse;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace ProgressionAgriculture
{
	public class CompUnlockCrop : ThingComp
	{
		public CompProperties_UnlockCrop Props => (CompProperties_UnlockCrop)props;
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (GameComponent_UnlockedCrops.Instance.IsCropUnlocked(Props.cropDef) is false)
			{
				yield return new FloatMenuOption("PA.UnlockCrop".Translate(Props.cropDef.label), () =>
				{
					Job job = JobMaker.MakeJob(PA_DefOf.UnlockCrop, parent);
					pawn.jobs.TryTakeOrderedJob(job);
				});
			}
			else
			{
				yield return new FloatMenuOption("PA.CropAlreadyUnlocked".Translate(Props.cropDef.label).CapitalizeFirst(), null);
			}
		}

		public void Unlock()
		{
			GameComponent_UnlockedCrops.Instance.UnlockCrop(Props.cropDef);
			parent.Destroy();
		}

        public override string TransformLabel(string label)
        {
			if (GameComponent_UnlockedCrops.Instance.IsCropUnlocked(Props.cropDef)) {
				return "PA.UnlockedSeedLabel".Translate(label);
			}
            return base.TransformLabel(label);
        }
	}
}