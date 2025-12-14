using HarmonyLib;
using RimWorld;
using Verse;

namespace LockWarrantsBehindResearch
{
    [DefOf]
    public static class Lock_DefOf
    {
        public static ResearchProjectDef WarrantNetwork;
    }

    public class MainButtonWorker_Warrants : MainButtonWorker_ToggleTab
    {
        public override bool Visible => base.Visible && Lock_DefOf.WarrantNetwork.IsFinished;
    }

    public class LockWarrantsBehindResearchMod : Mod
    {
        public LockWarrantsBehindResearchMod(ModContentPack pack) : base(pack)
        {
            new Harmony("LockWarrantsBehindResearchMod").PatchAll();
        }
    }
}
