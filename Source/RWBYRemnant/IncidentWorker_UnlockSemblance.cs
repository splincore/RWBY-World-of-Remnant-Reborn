﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RWBYRemnant
{
    public class IncidentWorker_UnlockSemblance : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().semblanceUnlockable) return false;
            Map map = (Map)parms.target;
            List<Pawn> pawnsWithAuraTrait = map.PlayerPawnsForStoryteller.ToList().FindAll(p => p.RaceProps.Humanlike && p.story.traits.HasTrait(RWBYDefOf.RWBY_Aura) && !p.WorkTagIsDisabled(WorkTags.Violent));
            pawnsWithAuraTrait.RemoveAll(p => p.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen));

            foreach (Pawn pawn in pawnsWithAuraTrait)
            {
                if (pawn.needs.mood.thoughts.memories.Memories.Any(m => SemblanceUtility.unlockSemblanceReasons.Contains(m.def.defName))) return true;
            }
            return false;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            List<Pawn> pawnsWithAuraTrait = map.PlayerPawnsForStoryteller.ToList().FindAll(p => p.RaceProps.Humanlike && p.story.traits.HasTrait(RWBYDefOf.RWBY_Aura) && !p.WorkTagIsDisabled(WorkTags.Violent));
            pawnsWithAuraTrait.RemoveAll(p => p.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen));
            pawnsWithAuraTrait.Shuffle();

            foreach (Pawn pawn in pawnsWithAuraTrait)
            {
                if (pawn.needs.mood.thoughts.memories.Memories.Any(m => SemblanceUtility.unlockSemblanceReasons.Contains(m.def.defName)))
                {
                    return pawn.TryGetComp<CompAura>() is CompAura compAura && compAura.TryUnlockSemblanceWith(null, true);
                }
            }
            return false;
        }
    }
}
