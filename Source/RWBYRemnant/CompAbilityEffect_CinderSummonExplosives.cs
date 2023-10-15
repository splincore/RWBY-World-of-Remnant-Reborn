using RimWorld;
using Verse;
using Verse.AI;

namespace RWBYRemnant
{
    public class CompAbilityEffect_CinderSummonExplosives : CompAbilityEffect
    {
        public Pawn CasterPawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Thing is Pawn targetPawn && targetPawn != CasterPawn && CasterPawn.CanSee(targetPawn))
            {
                Thing explosiveMine = ThingMaker.MakeThing(RWBYDefOf.Cinder_ExplosiveMine);
                GenSpawn.Spawn(explosiveMine, targetPawn.Position, targetPawn.Map);
                explosiveMine.TryGetComp<CompExplosiveSilent>().StartWick();
                base.Apply(target, dest);
            }
        }
    }
}
