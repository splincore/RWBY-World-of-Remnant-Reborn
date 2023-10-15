using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_RavenFromBond : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                if (target.Thing is Pawn targetPawn && targetPawn.RaceProps.Humanlike)
                {
                    if (parent.pawn.TryGetComp<CompAura>() is CompAura compAura && compAura.aura is Aura_Raven aura_Raven)
                    {
                        aura_Raven.RegisterBondedPawn(targetPawn);
                    }
                }
                base.Apply(target, dest);
            }
        }
    }
}
