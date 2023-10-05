using RimWorld;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class Projectile_AdamSlice : Bullet
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (launcher.TryGetComp<CompAura>() is CompAura compAura && compAura.aura is Aura_Adam aura_Adam)
            {
                DamageInfo extraDinfo = new DamageInfo(DamageDefOf.Cut, aura_Adam.absorbedDamage, 0f, -1f, launcher, null, launcher.def, DamageInfo.SourceCategory.ThingOrUnknown, hitThing);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                Vector3 direction = (hitThing.Position - launcher.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                hitThing.TakeDamage(extraDinfo);
                aura_Adam.absorbedDamage = 0;
            }
            base.Impact(hitThing);
        }
    }
}
