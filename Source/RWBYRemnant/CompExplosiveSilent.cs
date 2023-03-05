using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompExplosiveSilent : CompExplosive
    {
        public override void PostDraw()
        {
        }

        public override void CompTick()
        {
            if (wickStarted)
            {
                wickTicksLeft--;
                if (wickTicksLeft <= 0)
                {
                    Detonate(parent.MapHeld);
                }
            }
        }

        public void StartWick()
        {
            if (wickStarted)
            {
                return;
            }
            if (ExplosiveRadius() <= 0f)
            {
                return;
            }
            wickStarted = true;
            wickTicksLeft = Props.wickTicks.RandomInRange;
            GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(parent, Props.explosiveDamageType, null);
        }
    }
}
