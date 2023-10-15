using EquipmentToolbox;
using RimWorld;
using Verse;
using Verse.AI;

namespace RWBYRemnant
{
    public class CompAbilityEffect_CinderShootFireCrystal : CompAbilityEffect
    {
        public Pawn CasterPawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Thing is Pawn targetPawn && targetPawn != CasterPawn && CasterPawn.CanSee(targetPawn))
            {
                if (CasterPawn.apparel.WornApparel.Find(b => b.def == RWBYDefOf.Apparel_PowderedDustBelt) is ThingWithComps belt && belt.AllComps.Find(c => c is CompThingAbility tmpCompThingAbility && tmpCompThingAbility.AmmoDef == RWBYDefOf.FireDustPowder) is CompThingAbility compThingAbility && compThingAbility.ConsumeAmmo())
                {
                    Projectile projectile = (Projectile)GenSpawn.Spawn(RWBYDefOf.Cinder_FireCrystal, CasterPawn.Position, CasterPawn.Map);
                    projectile.Launch(CasterPawn, targetPawn, targetPawn, ProjectileHitFlags.IntendedTarget);
                    base.Apply(target, dest);
                }
            }
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (CasterPawn.apparel.WornApparel.Find(b => b.def == RWBYDefOf.Apparel_PowderedDustBelt) is ThingWithComps belt)
            {
                if (belt.AllComps.Find(c => c is CompThingAbility tmpCompThingAbility && tmpCompThingAbility.AmmoDef == RWBYDefOf.FireDustPowder) is CompThingAbility compThingAbility && compThingAbility.HasAmmoRemaining)
                {
                    return base.GizmoDisabled(out reason);
                }
            }
            reason = "CinderShootCrystalsReason".Translate(CasterPawn.Name.ToStringShort);
            return true;
        }
    }
}
