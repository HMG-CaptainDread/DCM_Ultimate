using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ProgressionAgriculture
{
	[HarmonyPatch]
	public static class IPlantToGrowSettable_GetPlantDefToGrow_Patch
	{
		public static IEnumerable<MethodBase> TargetMethods()
		{
			var interfaceType = typeof(IPlantToGrowSettable);
			foreach (var type in GenTypes.AllTypes)
			{
				if (type != interfaceType && interfaceType.IsAssignableFrom(type))
				{
					var method = AccessTools.DeclaredMethod(type, "GetPlantDefToGrow");
					if (method != null)
					{
						yield return method;
					}
				}
			}
		}

		public static bool allowVanilla;

		public static void Postfix(ref ThingDef __result)
		{
			if (allowVanilla) return;
			if (__result != null)
			{
				if (GameComponent_UnlockedCrops.Instance.IsCropUnlocked(__result) is false)
				{
					__result = null;
				}
			}
		}
	}
}
