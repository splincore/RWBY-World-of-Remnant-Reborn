using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace RWBYRemnant
{
    public class Aura_Yang : Aura
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
            if (absorbedDamage > 75f)
            {
                Hediff returnDamageHediff = HediffMaker.MakeHediff(RWBYDefOf.RWBY_YangReturnDamage, pawn);
                pawn.health.AddHediff(returnDamageHediff);
            }
            base.Tick();
        }

        public override bool TryAbsorbDamage(DamageInfo dinfo)
        {
            if (dinfo.Def.defName == "PJ_ForceHealDamage" || dinfo.Def == DamageDefOf.SurgicalCut || dinfo.Def == DamageDefOf.Extinguish) return base.TryAbsorbDamage(dinfo);
            absorbedDamage += dinfo.Amount;
            if (absorbedDamage > 100f) absorbedDamage = 100f;
            return base.TryAbsorbDamage(dinfo);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            string label = "";
            if (absorbedDamage == 0f)
            {
                label = "AngerLevelNotAngryLabel".Translate().CapitalizeFirst();
            }
            else if (absorbedDamage <= 25f)
            {
                label = "AngerLevelAnnoyedLabel".Translate().CapitalizeFirst();
            }
            else if (absorbedDamage <= 50f)
            {
                label = "AngerLevelAngryLabel".Translate().CapitalizeFirst();
            }
            else if (absorbedDamage <= 75f)
            {
                label = "AngerLevelRagingLabel".Translate().CapitalizeFirst();
            }
            else
            {
                label = "AngerLevelLostHairLabel".Translate().CapitalizeFirst();
            }
            yield return new GizmoYangAngerLevel
            {
                auraYang = this,
                label = label,
                FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(GetColor())
            };
            Command_Toggle activateDamageReturn = new Command_Toggle
            {
                defaultLabel = "YangReturnDamageLabel".Translate(),
                defaultDesc = "YangReturnDamageDescription".Translate(),
                icon = TexturesRWBY.UiAbility_YangReturn,
                disabled = false,
                isActive = () => pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_YangReturnDamage),
                toggleAction = delegate ()
                {
                    if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_YangReturnDamage))
                    {
                        pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_YangReturnDamage));
                    }
                    else
                    {
                        Hediff returnDamageHediff = HediffMaker.MakeHediff(RWBYDefOf.RWBY_YangReturnDamage, pawn);
                        pawn.health.AddHediff(returnDamageHediff);
                    }
                }
            };
            yield return activateDamageReturn;
        }

        public override Color GetColor()
        {
            return color;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<float>(ref absorbedDamage, "absorbedDamage", 0, false);
            base.ExposeData();
        }

        public float absorbedDamage = 0f;
        public Color color = new Color(0.8f, 0.8f, 0f);
    }
}
