using System.Collections.Generic;
using Verse;

namespace RWBYRemnant
{
    public class Aura_Hazel : Aura
    {
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            Command_Toggle activateIgnorePain = new Command_Toggle
            {
                defaultLabel = "HazelIgnorePainLabel".Translate(),
                defaultDesc = "HazelIgnorePainDescription".Translate(),
                icon = TexturesRWBY.UiAbility_IgnorePain,
                disabled = false,
                isActive = () => pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_IgnorePain),
                toggleAction = delegate ()
                {
                    if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_IgnorePain))
                    {
                        pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_IgnorePain));
                    }
                    else
                    {
                        Hediff ignorePainHediff = HediffMaker.MakeHediff(RWBYDefOf.RWBY_IgnorePain, pawn);
                        pawn.health.AddHediff(ignorePainHediff);
                    }
                }
            };
            yield return activateIgnorePain;
        }
    }
}
