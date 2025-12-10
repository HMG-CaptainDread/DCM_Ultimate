using RimWorld;
using SyControlsBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace CleaningLimbs
{
	public class CleaningLimbsSettings: ModSettings
	{
		#region CONSTANTS
		public const int Default_HandAdditionalCleans = 1;
		public const float Default_HandAdditionalSpeed = 0.25f;

		public const int Default_FootAdjacentCleans = 2;
		public const float Default_FootAdditionalSpeed = 0.25f;

		public const bool Default_WarCrimesMode = false;
		#endregion

		#region PROPERTIES
		public int HandAdditionalCleans { get; set; } = Default_HandAdditionalCleans;
		public float HandAdditionalSpeed { get; set; } = Default_HandAdditionalSpeed;
		private float _handPartEfficiency = Default_HandPartEfficiency;
		public float HandPartEfficiency
		{
			get => _handPartEfficiency;
			set => Util.SetValue(ref _handPartEfficiency, value, v =>
			{
				SetPartEfficiency(HediffDefOfCleaningLimbs.VacuumArm, v);
				SetPartEfficiency(HediffDefOfCleaningLimbs.VacuumHand, v);
			});
		}
		private float _handMovementSpeed = Default_HandMovementSpeed;
		public float HandMovementSpeed
		{
			get => _handMovementSpeed;
			set => Util.SetValue(ref _handMovementSpeed, value, v =>
			{
				SetMovementSpeed(HediffDefOfCleaningLimbs.VacuumArm, v);
				SetMovementSpeed(HediffDefOfCleaningLimbs.VacuumHand, v);
			});
		}

		public int FootAdjacentCleans { get; set; } = Default_FootAdjacentCleans;
		public float FootAdditionalSpeed { get; set; } = Default_FootAdditionalSpeed;
		private float _footPartEfficiency = Default_FootPartEfficiency;
		public float FootPartEfficiency
		{
			get => _footPartEfficiency;
			set => Util.SetValue(ref _footPartEfficiency, value, v =>
			{
				SetPartEfficiency(HediffDefOfCleaningLimbs.MopLeg, v);
				SetPartEfficiency(HediffDefOfCleaningLimbs.MopFoot, v);
			});
		}
		private float _footMovementSpeed = Default_FootMovementSpeed;
		public float FootMovementSpeed
		{
			get => _footMovementSpeed;
			set => Util.SetValue(ref _footMovementSpeed, value, v =>
			{
				SetMovementSpeed(HediffDefOfCleaningLimbs.MopLeg, v);
				SetMovementSpeed(HediffDefOfCleaningLimbs.MopFoot, v);
			});
		}

		public bool WarCrimesMode { get; set; } = Default_WarCrimesMode;
		private float _horribleLimbsMoodEffect = Default_HorribleLimbsMoodEffect;
		public float HorribleLimbsMoodEffect
		{
			get => _horribleLimbsMoodEffect;
			set => Util.SetValue(ref _horribleLimbsMoodEffect, value, v =>
			{
				SetMoodEffect(ThoughtDefOfCleaningLimbs.HorribleCleaningLimbs, v);
			});
		}
		#endregion

		#region FIELDS
		public static readonly float Default_HandPartEfficiency;
		public static readonly float Default_HandMovementSpeed;

		public static readonly float Default_FootPartEfficiency;
		public static readonly float Default_FootMovementSpeed;

		public static readonly float Default_HorribleLimbsMoodEffect;
		#endregion

		#region CONSTRUCTORS
		static CleaningLimbsSettings()
		{
			Default_HandPartEfficiency = HediffDefOfCleaningLimbs.VacuumHand.addedPartProps.partEfficiency;
			Default_HandMovementSpeed = GetMovementSpeedCapacityModifier(HediffDefOfCleaningLimbs.VacuumHand).offset;

			Default_FootPartEfficiency = HediffDefOfCleaningLimbs.MopFoot.addedPartProps.partEfficiency;
			Default_FootMovementSpeed = GetMovementSpeedCapacityModifier(HediffDefOfCleaningLimbs.MopFoot).offset;

			Default_HorribleLimbsMoodEffect = ThoughtDefOfCleaningLimbs.HorribleCleaningLimbs.stages[0].baseMoodEffect;
		}
		#endregion

		#region PUBLIC METHODS
		public void DoSettingsWindowContents(Rect inRect)
		{
			var width = inRect.width;
			var offsetY = 0.0f;

			ControlsBuilder.Begin(inRect);
			try
			{
				// Hand
				HandAdditionalCleans = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.HandAdditionalCleans".Translate(),
					"SY_CL.TooltipHandAdditionalCleans".Translate(),
					HandAdditionalCleans,
					Default_HandAdditionalCleans,
					nameof(HandAdditionalCleans),
					0,
					20);
				HandAdditionalSpeed = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.HandAdditionalSpeed".Translate(),
					"SY_CL.TooltipHandAdditionalSpeed".Translate(),
					HandAdditionalSpeed,
					Default_HandAdditionalSpeed,
					nameof(HandAdditionalSpeed),
					0f,
					10f);
				HandPartEfficiency = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.HandPartEfficiency".Translate(),
					"SY_CL.TooltipHandPartEfficiency".Translate(),
					HandPartEfficiency,
					Default_HandPartEfficiency,
					nameof(HandPartEfficiency),
					0f,
					1000f);
				HandMovementSpeed = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.HandMovementSpeed".Translate(),
					"SY_CL.TooltipHandMovementSpeed".Translate(),
					HandMovementSpeed,
					Default_HandMovementSpeed,
					nameof(HandMovementSpeed),
					-1f,
					1f);

				// Foot
				FootAdjacentCleans = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.FootAdjacentCleans".Translate(),
					"SY_CL.TooltipFootAdjacentCleans".Translate(),
					FootAdjacentCleans,
					Default_FootAdjacentCleans,
					nameof(FootAdjacentCleans),
					0,
					20);
				FootAdditionalSpeed = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.FootAdditionalSpeed".Translate(),
					"SY_CL.TooltipFootAdditionalSpeed".Translate(),
					FootAdditionalSpeed,
					Default_FootAdditionalSpeed,
					nameof(FootAdditionalSpeed),
					0f,
					10f);
				FootPartEfficiency = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.FootPartEfficiency".Translate(),
					"SY_CL.TooltipFootPartEfficiency".Translate(),
					FootPartEfficiency,
					Default_FootPartEfficiency,
					nameof(FootPartEfficiency),
					0f,
					1000f);
				FootMovementSpeed = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.FootMovementSpeed".Translate(),
					"SY_CL.TooltipFootMovementSpeed".Translate(),
					FootMovementSpeed,
					Default_FootMovementSpeed,
					nameof(FootMovementSpeed),
					-1f,
					1f);

				// War crimes
				WarCrimesMode = ControlsBuilder.CreateCheckbox(
					ref offsetY,
					width,
					"SY_CL.WarCrimesMode".Translate(),
					"SY_CL.TooltipWarCrimesMode".Translate(),
					WarCrimesMode,
					Default_WarCrimesMode);
				HorribleLimbsMoodEffect = ControlsBuilder.CreateNumeric(
					ref offsetY,
					width,
					"SY_CL.HorribleLimbsMoodEffect".Translate(),
					"SY_CL.TooltipHorribleLimbsMoodEffect".Translate(),
					HorribleLimbsMoodEffect,
					Default_HorribleLimbsMoodEffect,
					nameof(HorribleLimbsMoodEffect),
					-100f,
					100f);
			}
			finally
			{
				ControlsBuilder.End(offsetY);
			}
		}
		#endregion

		#region OVERRIDES
		public override void ExposeData()
		{
			base.ExposeData();

			bool boolValue;
			int intValue;
			float floatValue;

			// Hand
			intValue = HandAdditionalCleans;
			Scribe_Values.Look(ref intValue, nameof(HandAdditionalCleans), Default_HandAdditionalCleans);
			HandAdditionalCleans = intValue;
			floatValue = HandAdditionalSpeed;
			Scribe_Values.Look(ref floatValue, nameof(HandAdditionalSpeed), Default_HandAdditionalSpeed);
			HandAdditionalSpeed = floatValue;
			floatValue = HandPartEfficiency;
			Scribe_Values.Look(ref floatValue, nameof(HandPartEfficiency), Default_HandPartEfficiency);
			HandPartEfficiency = floatValue;
			floatValue = HandMovementSpeed;
			Scribe_Values.Look(ref floatValue, nameof(HandMovementSpeed), Default_HandMovementSpeed);
			HandMovementSpeed = floatValue;

			// Foot
			intValue = FootAdjacentCleans;
			Scribe_Values.Look(ref intValue, nameof(FootAdjacentCleans), Default_FootAdjacentCleans);
			FootAdjacentCleans = intValue;
			floatValue = FootAdditionalSpeed;
			Scribe_Values.Look(ref floatValue, nameof(FootAdditionalSpeed), Default_FootAdditionalSpeed);
			FootAdditionalSpeed = floatValue;
			floatValue = FootPartEfficiency;
			Scribe_Values.Look(ref floatValue, nameof(FootPartEfficiency), Default_FootPartEfficiency);
			FootPartEfficiency = floatValue;
			floatValue = FootMovementSpeed;
			Scribe_Values.Look(ref floatValue, nameof(FootMovementSpeed), Default_FootMovementSpeed);
			FootMovementSpeed = floatValue;

			// War crimes
			boolValue = WarCrimesMode;
			Scribe_Values.Look(ref boolValue, nameof(WarCrimesMode), Default_WarCrimesMode);
			WarCrimesMode = boolValue;
			floatValue = HorribleLimbsMoodEffect;
			Scribe_Values.Look(ref floatValue, nameof(HorribleLimbsMoodEffect), Default_HorribleLimbsMoodEffect);
			HorribleLimbsMoodEffect = floatValue;
		}
		#endregion

		#region PRIVATE METHODS
		private static void SetMoodEffect(ThoughtDef thoughtDef, float value)
		{
			for (int i = 0; i < thoughtDef.stages.Count; i++)
				thoughtDef.stages[i].baseMoodEffect = value * (i + 1);
		}

		private static void SetPartEfficiency(HediffDef hediffDef, float value) =>
			hediffDef.addedPartProps.partEfficiency = value;

		private static void SetMovementSpeed(HediffDef hediffDef, float value) =>
			GetMovementSpeedCapacityModifier(hediffDef).offset = value;

		private static PawnCapacityModifier GetMovementSpeedCapacityModifier(HediffDef hediffDef) =>
			hediffDef.stages[0].capMods.First((capMod) => capMod.capacity == PawnCapacityDefOf.Moving);
		#endregion
	}
}
