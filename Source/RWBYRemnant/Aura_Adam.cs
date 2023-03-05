using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace RWBYRemnant
{
    public class Aura_Adam : Aura
    {
        public override void Tick()
        {
            if (pawn.Downed)
            {
                absorbedDamage = 0f;
            }
            if (!pawn.IsFighting() && pawn.IsHashIntervalTick(120))
            {
                absorbedDamage -= 1f;
                if (absorbedDamage < 0f) absorbedDamage = 0f;
            }
            base.Tick();
        }

        public override bool TryAbsorbDamage(DamageInfo dinfo)
        {
            if (dinfo.Def.defName == "PJ_ForceHealDamage") return base.TryAbsorbDamage(dinfo);
            if (absorbedDamage < 500f && Rand.Chance(0.7f) && (pawn.Drafted || pawn.IsFighting()) && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsMeleeWeapon)
            {
                absorbedDamage += dinfo.Amount;
                if (absorbedDamage > 500f) absorbedDamage = 500f;
                RWBYDefOf.Draw_Gambol_Shroud_Katana.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                return true;
            }
            return base.TryAbsorbDamage(dinfo);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }

            string label = "AdamAbsorbDamageLabel".Translate().CapitalizeFirst();
            yield return new GizmoAdamAbsorbLevel
            {
                label = label,
                auraAdam = this,
                FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(GetColor())
            };
        }

        public override Color GetColor()
        {
            return color;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref absorbedDamage, "absorbedDamage", 0, false);
        }

        public float absorbedDamage = 0f;
        public Color color = new Color(1f, 0f, 0f);
    }
}
