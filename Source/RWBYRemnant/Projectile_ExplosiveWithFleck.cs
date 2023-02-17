using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RWBYRemnant
{
    public class Projectile_ExplosiveWithFleck : Projectile_Explosive
    {
        public ThingDef_ExplosiveFleckProjectile Def
        {
            get
            {
                return def as ThingDef_ExplosiveFleckProjectile;
            }
        }

        public override void Tick()
        {
            if (!Position.ToVector3().ShouldSpawnMotesAt(Map, true))
            {
                base.Tick();
                return;
            }
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(Position.ToVector3(), Map, FleckDefOf.DustPuffThick, 3);
            dataStatic.rotationRate = (float)Rand.Range(-60, 60);
            dataStatic.velocityAngle = (float)Rand.Range(0, 360);
            dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
            dataStatic.instanceColor = Def.color;
            Map.flecks.CreateFleck(dataStatic);
            base.Tick();
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (!Position.ToVector3().ShouldSpawnMotesAt(Map, true))
            {
                base.Impact(hitThing);
                return;
            }
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(Position.ToVector3(), Map, FleckDefOf.DustPuffThick, 5);
            dataStatic.rotationRate = (float)Rand.Range(-60, 60);
            dataStatic.velocityAngle = (float)Rand.Range(0, 360);
            dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
            dataStatic.instanceColor = Def.color;
            Map.flecks.CreateFleck(dataStatic);
            base.Impact(hitThing);
        }
    }
}
