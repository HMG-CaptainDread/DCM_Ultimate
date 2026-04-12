using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace ChickenExplosives
{
    public class CompProjectileSprayer : ThingComp
    {
        public CompProperties_ProjectileSprayer Props => (CompProperties_ProjectileSprayer)base.props;
        public bool fired = false;

        public void Fire()
        {
            if (fired) return;
            // throw projectiles
            var startingCells = Utils.GetStartingCells(Props, this.parent.Rotation, this.parent.OccupiedRect(), this.parent.Map);
            var affectedCells = Utils.GetAffectedCells(startingCells, Props, this.parent.Rotation, this.parent.OccupiedRect(), this.parent.Map);
            for (int i = 0; i < Props.projectileCount; i++)
            {
                IntVec3 spawnCell = startingCells.RandomElement();
                Projectile cur = GenSpawn.Spawn(Props.projectileDef, spawnCell, base.parent.Map) as Projectile;
                IntVec3 targetCell = affectedCells.RandomElement();
                cur.Launch(base.parent, targetCell, null, ProjectileHitFlags.All);
            }
            fired = true;
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            base.PostDrawExtraSelectionOverlays();
            GenDraw.DrawFieldEdges(Utils.GetTotalAffectedCells(this.Props, this.parent.Rotation, this.parent.OccupiedRect(), this.parent.Map).ToList(), GenTemperature.ColorRoomHot);
        }
    }
    public class CompProperties_ProjectileSprayer : CompProperties
    {
#pragma warning disable CS0649
        public ThingDef projectileDef;
        public int projectileCount;
        public IntRange projectileDistanceRange;
        public int projectileWidth;
#pragma warning restore CS0649

        public CompProperties_ProjectileSprayer()
        {
            base.compClass = typeof(CompProjectileSprayer);
        }
    }
}
