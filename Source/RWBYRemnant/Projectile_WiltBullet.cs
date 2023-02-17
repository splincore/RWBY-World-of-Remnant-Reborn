using EquipmentToolbox;
using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class Projectile_WiltBullet : Bullet
    {
        public ThingDef_WiltBullet Def
        {
            get
            {
                return def as ThingDef_WiltBullet;
            }
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (launcher is Pawn launcherPawn && launcherPawn != null)
            {
                launcherPawn.Position = Position;
                launcherPawn.Notify_Teleported(true, false);
                launcherPawn.equipment.Primary.TryGetComp<CompTransformable>().Transform();
            }
            base.Impact(hitThing);
        }
    }
}
