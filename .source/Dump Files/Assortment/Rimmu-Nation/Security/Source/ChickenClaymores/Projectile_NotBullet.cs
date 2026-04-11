using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ChickenExplosives
{
    /// <summary>
    /// Basically just a bullet, but with a custom BattleLogEntry which doesn't error on a null initiatorPawn
    /// </summary>
    internal class Projectile_NotBullet : Projectile
    {
        [Obsolete]
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            Map map = base.Map;
            base.Impact(hitThing, blockedByShield);
            // Battle log entry goes here
            // and here
            if (hitThing != null)
            {
                DamageInfo dinfo = new DamageInfo(base.def.projectile.damageDef, base.DamageAmount,
                                                  base.ArmorPenetration, ExactRotation.eulerAngles.y,
                                                  base.launcher, null, base.equipmentDef,
                                                  DamageInfo.SourceCategory.ThingOrUnknown, base.intendedTarget.Thing);
                hitThing.TakeDamage(dinfo); //associate with log
                if (hitThing is Pawn pawn && pawn.stances != null && pawn.BodySize <= base.def.projectile.stoppingPower + 0.001f)
                {
                    pawn.stances.StaggerFor(95);
                }

                if (base.def.projectile.extraDamages != null)
                {
                    foreach (ExtraDamage ed in base.def.projectile.extraDamages)
                    {
                        if (Rand.Chance(ed.chance))
                        {
                            DamageInfo dinfo2 = new DamageInfo(ed.def, ed.amount, ed.AdjustedArmorPenetration(),
                                                               ExactRotation.eulerAngles.y, base.launcher, null,
                                                               base.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, base.intendedTarget.Thing);
                            hitThing.TakeDamage(dinfo2); // associate with log
                        }
                    }
                }
            }
            else
            {
                SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map));
                if (base.Position.GetTerrain(map).takeSplashes)
                {
                    FleckMaker.WaterSplash(ExactPosition, map, Mathf.Sqrt(base.DamageAmount), 4f);
                }
                else
                {
                    FleckMaker.Static(ExactPosition, map, FleckDefOf.ShotHit_Dirt);
                }
            }
        }
    }
}
