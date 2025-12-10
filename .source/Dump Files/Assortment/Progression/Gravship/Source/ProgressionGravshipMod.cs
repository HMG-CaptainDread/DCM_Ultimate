using HarmonyLib;
using Verse;

namespace ProgressionGravship
{
    public class ProgressionGravshipMod : Mod
    {
        public ProgressionGravshipMod(ModContentPack pack) : base(pack)
        {
            new Harmony("ProgressionGravshipMod").PatchAll();
        }
    }
    
    
}
