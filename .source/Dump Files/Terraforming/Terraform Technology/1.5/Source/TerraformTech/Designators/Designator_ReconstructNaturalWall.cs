using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace TerraformTech
{
    public class Designator_ReconstructNaturalWall : Designator
    {
        public override int DraggableDimensions => 2;
        public override bool DragDrawMeasurements => false;

        protected override DesignationDef Designation => ResourceBank.DesignationDefOf.Designation_ReconstructNaturalWall;

        public Designator_ReconstructNaturalWall()
        {
            defaultLabel = ResourceBank.Strings.CommandReconstructNatural.Translate();
            defaultDesc = ResourceBank.Strings.CommandReconstructNaturalDesc.Translate();
            icon = ResourceBank.Textures.ReconstructNaturalWall;
            soundDragSustain = SoundDefOf.Designate_DragStandard;
            soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
            useMouseIcon = true;
            soundSucceeded = SoundDefOf.Designate_Deconstruct;
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(base.Map))
            {
                return false;
            }
            if (!DebugSettings.godMode && c.Fogged(base.Map))
            {
                return false;
            }
            if (TopReconstructableWallInCell(c, out AcceptanceReport reportToDisplay) == null)
            {
                return reportToDisplay;
            }
            return true;
        }

        public override void DesignateSingleCell(IntVec3 loc)
        {
            DesignateThing(TopReconstructableWallInCell(loc, out AcceptanceReport _));
        }

        private Thing TopReconstructableWallInCell(IntVec3 loc, out AcceptanceReport reportToDisplay)
        {
            reportToDisplay = AcceptanceReport.WasRejected;
            foreach (Thing item in from t in base.Map.thingGrid.ThingsAt(loc)
                                   orderby t.def.altitudeLayer descending
                                   select t)
            {
                AcceptanceReport acceptanceReport = CanDesignateThing(item);
                if (CanDesignateThing(item).Accepted)
                {
                    reportToDisplay = AcceptanceReport.WasAccepted;
                    return item;
                }
                if (!acceptanceReport.Reason.NullOrEmpty())
                {
                    reportToDisplay = acceptanceReport;
                }
            }
            return null;
        }

        public override void DesignateThing(Thing t)
        {
            if ((t.def.building?.isNaturalRock == true || (!(t.def.building?.artificialForMeditationPurposes == true) && !(t.def.building?.claimable == true))) && t.def.useHitPoints && t.def.category == ThingCategory.Building)
            {
                if (DebugSettings.godMode)
                {
                    t.HitPoints = t.MaxHitPoints;
                }

                base.Map.designationManager.AddDesignation(new Designation(t, Designation));
            }
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            if (!(t.def.building?.isNaturalRock == true || (!(t.def.building?.artificialForMeditationPurposes == true) && !(t.def.building?.claimable == true))) || !t.def.useHitPoints || (t.def.category != ThingCategory.Building))
            {
                return false;
            }
            if (base.Map.designationManager.DesignationOn(t, Designation) != null)
            {
                return false;
            }
            if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
            {
                return false;
            }
            return true;
        }

        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();
        }
    }
}
