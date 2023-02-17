using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class Projectile_WithFleck : Bullet
    {
        public ThingDef_FleckProjectile Def
        {
            get
            {
                return def as ThingDef_FleckProjectile;
            }
        }

        public override void Tick()
        {
            if (!Position.ToVector3().ShouldSpawnMotesAt(Map, true))
            {
                base.Tick();
                return;
            }
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(Position.ToVector3(), Map, FleckDefOf.DustPuffThick, 2);
            dataStatic.rotationRate = (float)Rand.Range(-60, 60);
            dataStatic.velocityAngle = (float)Rand.Range(0, 360);
            dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
            dataStatic.instanceColor = Def.color;
            Map.flecks.CreateFleck(dataStatic);
            base.Tick();
        }
    }
}
