using RimWorld;
using System.Linq;
using Verse;

namespace RWBYRemnant
{
    public class HediffComp_ApathyTiredness : HediffComp
    {
        public HediffCompProperties_ApathyTiredness Props => (HediffCompProperties_ApathyTiredness)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (!this.Pawn.IsHashIntervalTick(GenDate.TicksPerDay / 2)) return;
            bool apathyOnMap = false;
            if (this.Pawn.Map != null && this.Pawn.Map.mapPawns.AllPawnsSpawned.Any(p => p.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Apathy))
            {
                apathyOnMap = true;
            }

            if (apathyOnMap)
            {
                severityAdjustment += (0.1f * (1f - this.Pawn.needs.mood.CurInstantLevelPercentage));
            }
            else
            {
                severityAdjustment -= 1f;
            }
        }
    }
}
