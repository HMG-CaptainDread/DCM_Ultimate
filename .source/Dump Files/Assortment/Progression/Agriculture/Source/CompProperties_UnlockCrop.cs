using Verse;

namespace ProgressionAgriculture
{
    public class CompProperties_UnlockCrop : CompProperties
    {
        public ThingDef cropDef;
        public CompProperties_UnlockCrop()
        {
            compClass = typeof(CompUnlockCrop);
        }
    }
}