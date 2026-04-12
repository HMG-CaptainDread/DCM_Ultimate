using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace CleaningLimbs
{
	public class CleaningLimbs : Mod
	{
		#region PROPERTIES
		public static CleaningLimbs Instance { get; private set; }
		public static CleaningLimbsSettings Settings { get; private set; }
		#endregion

		#region CONSTRUCTORS
		public CleaningLimbs(ModContentPack content) : base(content)
		{
			Instance = this;

			LongEventHandler.ExecuteWhenFinished(Initialize);
		}
		#endregion

		#region OVERRIDES
		public override string SettingsCategory() =>
			"Cleaning Limbs";

		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);

			Settings.DoSettingsWindowContents(inRect);
		}
		#endregion

		#region PRIVATE METHODS
		private void Initialize()
		{
			Settings = GetSettings<CleaningLimbsSettings>();
		}
		#endregion
	}
}
