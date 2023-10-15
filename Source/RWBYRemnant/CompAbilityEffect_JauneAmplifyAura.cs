using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_JauneAmplifyAura : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Thing is Pawn targetPawn && targetPawn.RaceProps.Humanlike)
            {
                if (targetPawn.TryGetComp<CompAura>() is CompAura compAura && compAura.aura != null)
                {
                    if ((compAura.aura.CurrentEnergy + 50f) > compAura.aura.maxEnergy)
                    {
                        parent.pawn.TryGetComp<CompAura>().aura.CurrentEnergy += (compAura.aura.CurrentEnergy + 50f - compAura.aura.maxEnergy);
                    }
                    compAura.aura.CurrentEnergy += 50f;
                    Hediff amplifiedAura = HediffMaker.MakeHediff(RWBYDefOf.RWBY_AmplifiedAura, targetPawn);
                    targetPawn.health.AddHediff(amplifiedAura);
                }
            }
            base.Apply(target, dest);
        }
    }
}
