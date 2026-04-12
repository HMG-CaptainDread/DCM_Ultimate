using RimWorld;
using Verse;

namespace ProgressionAgriculture
{
	public class Command_SetPlantToGrowNothingToGrow : Command_SetPlantToGrow
	{
		public Command_SetPlantToGrowNothingToGrow()
		{
			defaultLabel = "PA.NothingToSowYet".Translate();
			icon = SetPlantToGrowTex;
		}
	}
}
