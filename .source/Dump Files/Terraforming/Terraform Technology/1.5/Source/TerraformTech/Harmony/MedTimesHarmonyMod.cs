using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace TerraformTech
{
    public sealed class TerraformTechHarmonyMod : Mod
    {
        public TerraformTechHarmonyMod(ModContentPack content) : base(content)
        {
            new Harmony("Vovikus.TerraformTechnology").PatchAll();
        }
    }
}
