using RimWorld;

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
            FleckMaker.ThrowDustPuffThick(Position.ToVector3(), Map, 2, Def.color);
            base.Tick();
        }
    }
}
