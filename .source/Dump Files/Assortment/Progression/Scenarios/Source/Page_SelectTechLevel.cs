using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;
using System;

namespace ProgressionScenarios
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class HotSwappableAttribute : Attribute
	{
	}

	[HotSwappable]
	[StaticConstructorOnStartup]
	public class Page_SelectTechLevel : Page
	{
		public override string PageTitle => "Choose your starting technology";
		private readonly TechLevel[] techLevels = Enum.GetValues(typeof(TechLevel)).Cast<TechLevel>().Where(t => t != TechLevel.Undefined).ToArray();
		public override Vector2 InitialSize
		{
			get
			{
				var size = base.InitialSize;
				return new Vector2(size.x, size.y - 75);
			}
		}
		public override void PreOpen()
		{
			base.PreOpen();
			ScenarioLister.MarkDirty();
		}

		public override void DoWindowContents(Rect rect)
		{
			DrawPageTitle(rect);
			Rect mainRect = GetMainRect(rect);
			Widgets.BeginGroup(mainRect);
			float boxWidth = 300;
			float boxHeight = 150;
			float spacing = 28;

			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Medium;
			var yOffset = 15;
			DrawTechLevelBox(new Rect((mainRect.width - boxWidth) / 2, yOffset, boxWidth, boxHeight), techLevels[0]);

			int columns = Mathf.FloorToInt(mainRect.width / (boxWidth + spacing));
			for (int i = 1; i < techLevels.Length; i++)
			{
				int row = (i - 1) / columns;
				int column = (i - 1) % columns;
				Rect boxRect = new Rect(column * (boxWidth + spacing) + 15, yOffset + boxHeight + spacing + row * (boxHeight + spacing), boxWidth, boxHeight);
				DrawTechLevelBox(boxRect, techLevels[i]);
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;

			Widgets.EndGroup();
			float y = rect.y + rect.height - 38f;
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			var backSize = 200;
			if ((Widgets.ButtonText(new Rect(((rect.x + rect.width) / 2f) - (backSize / 2), y, backSize, BottomButSize.y), label) || KeyBindingDefOf.Cancel.KeyDownEvent) && CanDoBack())
			{
				DoBack();
			}
		}

		private void DrawTechLevelBox(Rect boxRect, TechLevel techLevel)
		{
			Widgets.DrawBox(boxRect);
			var iconRect = boxRect.ContractedBy(25);
			iconRect.y -= 15;
			if (Widgets.ButtonImageFitted(iconRect, ContentFinder<Texture2D>.Get("UI/Icons/" + techLevel.ToString())))
			{
				ScenarioLister_ScenariosInCategory_Patch.selectedLevel = techLevel;
				if (ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef).Any() || 
					ScenarioLister.ScenariosInCategory(ScenarioCategory.CustomLocal).Any() ||
					ScenarioLister.ScenariosInCategory(ScenarioCategory.SteamWorkshop).Any())
				{
					this.Close();
					var page = new Page_SelectScenario();
					page.prev = this;
					Find.WindowStack.Add(page);
				}
				else
				{
					ScenarioLister_ScenariosInCategory_Patch.selectedLevel = TechLevel.Undefined;
					Messages.Message("No scenarios available for selected tech-level.", MessageTypeDefOf.RejectInput, historical: false);
				}
			}
			Widgets.Label(new Rect(boxRect.x, boxRect.y + (boxRect.height - 35), boxRect.width, 40), techLevel.ToString());
		}
	}
}
