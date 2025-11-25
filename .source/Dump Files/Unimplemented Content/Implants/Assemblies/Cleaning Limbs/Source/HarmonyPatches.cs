using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace CleaningLimbs
{
	[StaticConstructorOnStartup]
	public static class HarmonyPatches
	{
		static HarmonyPatches()
		{
			var harmony = new Harmony("syrus.cleaninglimbs");

			harmony.Patch(
				AccessTools.Method(typeof(JobDriver_CleanFilth), "MakeNewToils"),
				postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(JobDriver_CleanFilth_MakeNewToils_Postfix)));

			harmony.Patch(
				AccessTools.Method(typeof(JobDriver_ClearSnowAndSand), "MakeNewToils"),
				postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(JobDriver_ClearSnowAndSand_MakeNewToils_Postfix)));

			// CleaningArea patch
			var typeCleaningArea = Type.GetType("CleaningArea.JobDriver_CleanFilth_CleaningArea, CleaningArea");
			if (typeCleaningArea != null)
			{
				Log.Message("CleaningLimbs: applying CleaningArea.JobDriver_CleanFilth_CleaningArea patch");
				harmony.Patch(
					AccessTools.Method(typeCleaningArea, "MakeNewToils"),
					postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(JobDriver_CleanFilth_CleaningArea_MakeNewToils_Postfix)));
			}
		}

		static IEnumerable<Toil> JobDriver_CleanFilth_MakeNewToils_Postfix(IEnumerable<Toil> toils, JobDriver_CleanFilth __instance)
		{
			int c = toils.Count();
			foreach (var toil in toils)
			{
				if (--c == 1) // hopefully the cleaning job will always be the second to last job in the list...
					ReplaceCleanToilAction(toil, __instance);
				yield return toil;
			}
		}
		static IEnumerable<Toil> JobDriver_CleanFilth_CleaningArea_MakeNewToils_Postfix(IEnumerable<Toil> toils, JobDriver __instance)
		{
			int c = toils.Count();
			foreach (var toil in toils)
			{
				if (--c == 2) // once again, hoping the jobs stays at that position
					ReplaceCleanToilAction(toil, __instance);
				yield return toil;
			}
		}

		// function to replace the cleaning action
		static void ReplaceCleanToilAction(Toil toil, JobDriver __instance)
		{
			// count number of parts
			(int hands, int feet) = CountHandsAndFeet(__instance);

			// reflection because CleaningArea screws up how cleaning jobs are generated
			var instanceType = __instance.GetType();
			var filthFieldInfo = AccessTools.Property(instanceType, "Filth");
			var cleaningWorkDoneFieldInfo = AccessTools.Field(instanceType, "cleaningWorkDone");
			var totalCleaningWorkDoneFieldInfo = AccessTools.Field(instanceType, "totalCleaningWorkDone");

			// replace tick action
			toil.tickAction = delegate
			{
				// get filth
				var filth = (Filth)filthFieldInfo.GetValue(__instance);
				// vanilla - get cleaning time factor
				var timeFactor = filth.Position.GetTerrain(filth.Map).GetStatValueAbstract(StatDefOf.CleaningTimeFactor);
				// get cleaning speed & calculate work done in current iteration, adding hands & feet speed boost
				var workDone = __instance.pawn.GetStatValue(StatDefOf.CleaningSpeed) * (1f + hands * CleaningLimbs.Settings.HandAdditionalSpeed + feet * CleaningLimbs.Settings.FootAdditionalSpeed);

				// vanilla - cleaning time factor
				if (timeFactor != 0f)
					workDone /= timeFactor;

				// vanilla - get cleaning work done & add work done
				var cleaningWorkDone = (float)cleaningWorkDoneFieldInfo.GetValue(__instance) + workDone;
				// vanilla - get & set total cleaning work done
				totalCleaningWorkDoneFieldInfo.SetValue(__instance, (float)totalCleaningWorkDoneFieldInfo.GetValue(__instance) + workDone);

				// vanilla - insufficient work done to clean filth yet
				if (cleaningWorkDone <= filth.def.filth.cleaningWorkToReduceThickness)
					cleaningWorkDoneFieldInfo.SetValue(__instance, cleaningWorkDone);
				// vanilla - sufficient work done to clean filth
				else
				{
					// vanilla - base cleaning
					filth.ThinFilth();

					// vacuum hand'ing
					if (hands > 0)
						cleanHere(filth);

					// mop feet'ing
					if (feet > 0)
						cleanAdjacent(filth);

					// vanilla
					cleaningWorkDoneFieldInfo.SetValue(__instance, 0f);
					if (filth.Destroyed)
					{
						toil.actor.records.Increment(RecordDefOf.MessesCleaned);
						__instance.ReadyForNextToil();
					}
				}
			};

			void cleanHere(Filth filth)
			{
				var cleans = hands * CleaningLimbs.Settings.HandAdditionalCleans;
				// clean current tile; expending cleaning cycles on the current or additional filth
				cleanAt(filth, filth.positionInt, ref cleans);
			}
			void cleanAdjacent(Filth filth)
			{
				var cleans = feet * CleaningLimbs.Settings.FootAdjacentCleans;
				// clean filth from surrounding tiles
				foreach (var cell in GenAdj.AdjacentCellsAround)
				{
					if (cleanAt(filth, filth.positionInt + cell, ref cleans))
						return;
				}

				// use remaining available clean actions to clean filth at center
				cleans /= 2;
				while (!filth.Destroyed && cleans-- > 0)
					filth.ThinFilth();
			}
			bool cleanAt(Filth filth, IntVec3 positionInt, ref int cleans)
			{
				var things = positionInt.GetThingList(__instance.Map);
				for (int i = things.Count - 1; i >= 0; i--)
				{
					if (!(things[i] is Filth moreFilth))
						continue;
					
					// clean until filth is cleaned or number of cleans is used up
					while (!moreFilth.Destroyed)
					{
						// clean adjacent filth
						moreFilth.ThinFilth();

						// increase number of messes cleaned for pawn
						if (moreFilth != filth && moreFilth.Destroyed)
							toil.actor.records.Increment(RecordDefOf.MessesCleaned);

						// limit to max number of cleans
						if (--cleans == 0)
							return true;
					}
				}
				return false;
			}
		}

		static IEnumerable<Toil> JobDriver_ClearSnowAndSand_MakeNewToils_Postfix(IEnumerable<Toil> toils, JobDriver_ClearSnowAndSand __instance)
		{
			int c = toils.Count();
			foreach (var toil in toils)
			{
				if (--c == 0) // hopefully the clearing snow job will always be the last job in the list...
					ReplaceCleanToilAction(toil);
				yield return toil;
			}

			void ReplaceCleanToilAction(Toil toil)
			{
				// count number of parts
				(int hands, int feet) = CountHandsAndFeet(__instance);

				// replace tick action
				toil.tickAction = delegate
				{
					float statValue = toil.actor.GetStatValue(StatDefOf.GeneralLaborSpeed);
					__instance.workDone += statValue + hands * CleaningLimbs.Settings.HandAdditionalSpeed * 2;
					if (__instance.workDone >= __instance.TotalNeededWork)
					{
						var map = __instance.Map;
						var loc = __instance.TargetLocA;
						map.snowGrid.SetDepth(loc, 0f);

						if (feet > 0)
							CleanAdjacent(toil, map, loc, feet);

						__instance.ReadyForNextToil();
					}
				};
			}

			// function to clean adjacent tiles
			void CleanAdjacent(Toil toil, Map map, IntVec3 center, int feet)
			{
				var clears = feet * CleaningLimbs.Settings.FootAdjacentCleans;
				foreach (var cell in GenAdj.AdjacentCellsAround)
				{
					var loc = center + cell;
					// search for snow
					if (map.snowGrid.GetDepth(loc) > 0f)
					{
						// clear snow
						map.snowGrid.SetDepth(loc, 0f);

						// limit to max number of cleans
						if (--clears == 0)
							return;
					}
				}
			}
		}

		private static (int hands, int feet) CountHandsAndFeet(JobDriver jobDriver)
		{
			var list = new List<Hediff_AddedPart>();
			jobDriver.pawn.health.hediffSet.GetHediffs(ref list);
			int hands = 0, feet = 0;
			foreach (var hediff in list)
			{
				var name = hediff.def.defName;
				if (hediff.def == HediffDefOfCleaningLimbs.VacuumArm || hediff.def == HediffDefOfCleaningLimbs.VacuumHand)
					hands++;
				else if (hediff.def == HediffDefOfCleaningLimbs.MopLeg || hediff.def == HediffDefOfCleaningLimbs.MopFoot)
					feet++;
			}
			return (hands, feet);
		}
	}
}
