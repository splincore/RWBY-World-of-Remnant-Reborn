using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_PyrrhaUnlockAura : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                if (target.Thing is Pawn targetPawn && targetPawn.RaceProps.Humanlike && !targetPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen) && targetPawn.TryGetComp<CompAura>() is CompAura compAura && (compAura.aura == null || compAura.aura.CurrentEnergy < 10f))
                {
                    SemblanceUtility.TryUnlockAura(targetPawn, "LetterTextUnlockAuraPyrrha");
                }
                base.Apply(target, dest);
            }
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return base.Valid(target, throwMessages) && target.Thing is Pawn targetPawn && targetPawn.RaceProps.Humanlike && !targetPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen) && targetPawn.TryGetComp<CompAura>() is CompAura compAura && (compAura.aura == null || compAura.aura.CurrentEnergy < 10f);
        }
    }
}
