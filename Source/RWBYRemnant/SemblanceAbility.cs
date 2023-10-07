using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RWBYRemnant
{
    public class SemblanceAbility : Ability
    {
        public override bool CanCast
        {
            get
            {
                if (!base.CanCast) return false;
                return pawn.TryGetComp<CompAura>() is CompAura compAura && compAura.aura.CurrentEnergy >= def.AuraCost;
            }
        }

        public SemblanceAbility(Pawn pawn) : base(pawn)
        {
        }

        public SemblanceAbility(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
            this.def = (SemblanceAbilityDef)def;
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (pawn.TryGetComp<CompAura>() is CompAura compAura)
            {
                if (compAura.aura.CurrentEnergy < def.AuraCost)
                {
                    reason = "SemblanceAbilityCastNoAura".Translate().Formatted(pawn.Named("PAWN"));
                    return true;
                }
            }
            else
            {
                reason = "SemblanceAbilityCastNoAura".Translate().Formatted(pawn.Named("PAWN"));
                return true;
            }
            return base.GizmoDisabled(out reason);
        }

        public override bool Activate(GlobalTargetInfo target)
        {
            return ConsumeAura() && base.Activate(target);
        }

        public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return ConsumeAura() && base.Activate(target, dest);
        }

        public bool ConsumeAura()
        {
            if (pawn.TryGetComp<CompAura>() is CompAura compAura)
            {
                return compAura.aura.TryConsumeAura(def.AuraCost);
            }
            else
            {
                return false;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<SemblanceAbilityDef>(ref this.def, "def");
        }

        public new SemblanceAbilityDef def;
    }
}
