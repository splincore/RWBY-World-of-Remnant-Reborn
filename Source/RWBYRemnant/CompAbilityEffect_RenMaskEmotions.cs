using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_RenMaskEmotions : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Thing is Pawn targetPawn && targetPawn.RaceProps.Humanlike)
            {
                Hediff maskedEmotions = HediffMaker.MakeHediff(RWBYDefOf.RWBY_MaskedEmotions, targetPawn);
                targetPawn.health.AddHediff(maskedEmotions);
            }
            base.Apply(target, dest);
        }
    }
}
