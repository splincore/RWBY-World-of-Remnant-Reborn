using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_RubyCarry : CompAbilityEffect_WithDest
    {
        public new CompProperties_RubyCarry Props => (CompProperties_RubyCarry)props;

        public override bool CanHitTarget(LocalTargetInfo target)
        {
            return base.CanPlaceSelectedTargetAt(target) && base.CanHitTarget(target);
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                base.Apply(target, dest);
                LocalTargetInfo destination = base.GetDestination(dest.IsValid ? dest : target);
                if (destination.IsValid)
                {
                    Pawn pawn = this.parent.pawn;
                    Pawn carriedPawn = target.Thing as Pawn;
                    Projectile_RubyCarry projectile = (Projectile_RubyCarry)GenSpawn.Spawn(Props.projectileDef, pawn.Position, pawn.Map);
                    projectile.carriedPawn = carriedPawn;
                    projectile.Launch(pawn, destination, destination, ProjectileHitFlags.IntendedTarget);
                }
            }
        }
    }
}
