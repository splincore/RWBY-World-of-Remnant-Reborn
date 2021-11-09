using Verse;

namespace RWBYRemnant
{
    public class HediffComp_FrozenWorker : HediffComp
    {
        public HediffCompProperties_FrozenWorker Props => (HediffCompProperties_FrozenWorker)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (parent.CurStageIndex == 2) parent.pawn.stances.stunner.StunFor(GenTicks.SecondsToTicks(1f), null);
        }
    }
}
