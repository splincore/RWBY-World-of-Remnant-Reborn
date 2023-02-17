using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class Projectile_CrystalBullet : Bullet
    {
        public ThingDef_CrystalBullet Def
        {
            get
            {
                return def as ThingDef_CrystalBullet;
            }
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            GenSpawn.Spawn(Def.spawnOnImpact, Position, Map);
            Destroy(DestroyMode.Vanish);
        }
    }
}
