using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_RenMaskEmotionsAmplified : CompAbilityEffect_RenMaskEmotions
    {
        public Pawn CasterPawn => parent.pawn;
        public override bool GizmoDisabled(out string reason)
        {
            if (!CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AmplifiedAura))
            {
                reason = "MaskEmotionAmplifiedReason".Translate(CasterPawn.Name.ToStringShort);
                return true;
            }
            return base.GizmoDisabled(out reason);
        }
    }
}
