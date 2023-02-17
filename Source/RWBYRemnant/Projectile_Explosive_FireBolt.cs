using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class Projectile_Explosive_FireBolt : Projectile_Explosive
    {
        public ThingDef_Myrtenaster_FireBolt Def
        {
            get
            {
                return def as ThingDef_Myrtenaster_FireBolt;
            }
        }

        public int currentTick = 0;

        public override void Tick()
        {
            if (currentTick > Def.fireDistanceInTicks && Find.TickManager.TicksGame % 2 == 0)
            {
                FilthMaker.TryMakeFilth(Position, Map, Def.spawnWhileFlying);
                Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire);
                fire.fireSize = 1f;
                GenSpawn.Spawn(fire, Position, Map);
            }
            else
            {
                currentTick++;
            }
            base.Tick();
        }
    }
}
