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
            Thing temp = GenSpawn.Spawn(Def.spawnOnImpact, Position, Map);
            temp.SetForbidden(true);
            Destroy(DestroyMode.Vanish);
        }
    }
}
