﻿using EquipmentToolbox;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RWBYRemnant
{
    public class Aura : IExposable
    {
        public Aura()
        {
            
        }

        public virtual void Tick()
        {
            if (!pawn.IsFighting() && pawn.IsHashIntervalTick(480)) // every 8 seconds Aura can either heal or regenerate IF pawn is out of combat
            {
                List<Hediff_Injury> injuriesHealable = new List<Hediff_Injury>();
                foreach (Hediff hediff_Injury in pawn.health.hediffSet.hediffs)
                {
                    if (hediff_Injury.GetType().Equals(typeof(Hediff_Injury)) && ((Hediff_Injury)hediff_Injury).TendableNow(true))
                    {
                        injuriesHealable.Add((Hediff_Injury)hediff_Injury);
                    }
                }
                if (injuriesHealable.Count > 0 && TryConsumeAura(1f)) // try heal
                {
                    injuriesHealable.RandomElement().Heal(1f);
                }
                else // regen Aura
                {
                    // injured pawns won´t regenerate aura if aura is empty, regenerates only out of combat and last taken damage is 60 seconds away
                    if (CurrentEnergy < maxEnergy && Find.TickManager.TicksGame - lastAbsorbDamageTick > GenTicks.SecondsToTicks(60f)) CurrentEnergy += 1f;
                }
            }

            if (pawn.IsHashIntervalTick(60) && (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AmplifiedAura) || pawn.health.hediffSet.hediffs.Any(h => SemblanceUtility.injectedDustCrystalHediffs.Contains(h.def)))) // amplified Aura heals faster
            {
                int healAmount = 0;
                if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AmplifiedAura))
                {
                    healAmount += 3;
                }
                if (pawn.health.hediffSet.hediffs.Any(h => SemblanceUtility.injectedDustCrystalHediffs.Contains(h.def)))
                {
                    healAmount += pawn.health.hediffSet.hediffs.FindAll(h => SemblanceUtility.injectedDustCrystalHediffs.Contains(h.def)).Sum(s => s.CurStageIndex + 1);
                }
                List<Hediff_Injury> injuriesHealable = new List<Hediff_Injury>();
                foreach (Hediff hediff_Injury in pawn.health.hediffSet.hediffs)
                {
                    if (hediff_Injury.GetType().Equals(typeof(Hediff_Injury)) && ((Hediff_Injury)hediff_Injury).TendableNow(true))
                    {
                        injuriesHealable.Add((Hediff_Injury)hediff_Injury);
                    }
                }
                if (injuriesHealable.Count > 0)
                {
                    for (int a = 0; a < healAmount; a++)
                    {
                        if (TryConsumeAura(1f)) injuriesHealable.RandomElement().Heal(1f);
                    }
                }
            }

            if (pawn.IsHashIntervalTick(30) && pawn.health.hediffSet.hediffs.Any(h => SemblanceUtility.injectedDustCrystalHediffs.Contains(h.def))) // with Dust injected Aura regenerates faster
            {
                float energyToRegenerate = pawn.health.hediffSet.hediffs.FindAll(h => SemblanceUtility.injectedDustCrystalHediffs.Contains(h.def)).Sum(s => s.CurStageIndex + 1);
                if (CurrentEnergy < maxEnergy) CurrentEnergy += energyToRegenerate;
            }

            if (CurrentEnergy > maxEnergy) CurrentEnergy = maxEnergy;
        }

        public virtual void TickRare()
        {

        }

        public virtual bool TryAbsorbDamage(DamageInfo dinfo)
        {
            if (CurrentEnergy == 0f || dinfo.Def == DamageDefOf.SurgicalCut || dinfo.Def == DamageDefOf.Extinguish) return false;
            if (dinfo.Def.defName == "PJ_ForceHealDamage") return false;
            if (!pawn.Drafted && !pawn.IsFighting() && Rand.Chance(0.05f))
            {
                if (pawn.IsColonist)
                {
                    Messages.Message("MessageTextForgotAura".Translate().Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN").CapitalizeFirst(), pawn, MessageTypeDefOf.NegativeEvent);
                }
                return false;
            }

            if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AmplifiedAura))
            {
                CurrentEnergy -= dinfo.Amount / 2f; // amplified Aura takes half the damage
            }
            else
            {
                CurrentEnergy -= dinfo.Amount;
            }

            if (CurrentEnergy > 0f) // normal absorb
            {
                if (pawn.Spawned)
                {
                    RWBYDefOf.AuraFlicker.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                    impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
                    Vector3 loc = pawn.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
                    float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
                    FleckMaker.Static(loc, pawn.Map, FleckDefOf.ExplosionFlash, num);
                    int num2 = (int)num;
                    for (int i = 0; i < num2; i++)
                    {
                        FleckMaker.ThrowDustPuff(loc, pawn.Map, Rand.Range(0.8f, 1.2f));
                    }
                }
            }
            else // break
            {
                if (pawn.Spawned)
                {
                    RWBYDefOf.AuraBreak.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                    FleckMaker.Static(pawn.TrueCenter(), pawn.Map, FleckDefOf.ExplosionFlash, 12f);
                    for (int i = 0; i < 6; i++)
                    {
                        Vector3 loc = pawn.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f);
                        FleckMaker.ThrowDustPuff(loc, pawn.Map, Rand.Range(0.8f, 1.2f));
                    }
                    CurrentEnergy = 0f;
                }
            }
            lastAbsorbDamageTick = Find.TickManager.TicksGame;
            return true;
        }

        public bool TryConsumeAura(float consumeAmount)
        {
            if (CurrentEnergy >= consumeAmount)
            {
                CurrentEnergy -= consumeAmount;
                return true;
            }
            return false;
        }

        public float CurrentEnergy
        {
            get => currentEnergy;
            set
            {
                currentEnergy = value;
                if (currentEnergy < 0)
                {
                    currentEnergy = 0;
                }
                else if (currentEnergy > maxEnergy)
                {
                    currentEnergy = maxEnergy;
                }
            }
        }

        public bool Active()
        {
            return pawn.Drafted && CurrentEnergy > 0 && !pawn.Downed && !pawn.Dead;
        }

        public virtual IEnumerable<Gizmo> GetGizmos()
        {
            yield return new GizmoAuraStatus
            {
                aura = this,
                label = "AuraGizmoLabel".Translate(pawn.Name.ToStringShort),
                FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(GetColor())
            };
            if (pawn.apparel.WornApparel.Find(b => b.def == RWBYDefOf.Apparel_DustCrystalBelt) is ThingWithComps belt)
            {
                foreach (CompThingAbility tmpCompThingAbility in belt.AllComps.FindAll(c => c is CompThingAbility).Cast<CompThingAbility>())
                {
                    yield return new Command_Action()
                    {
                        defaultLabel = "InjectCrystalLabel".Translate(),
                        defaultDesc = "InjectCrystalDescription".Translate(tmpCompThingAbility.AmmoDef.label),
                        icon = tmpCompThingAbility.AmmoDef.uiIcon,
                        defaultIconColor = tmpCompThingAbility.AmmoDef.uiIconColor,
                        
                        disabled = !tmpCompThingAbility.HasAmmoRemaining || pawn.Downed,
                        disabledReason = pawn.Downed ? "InjectCrystalDowned".Translate(pawn.LabelShort) : "InjectCrystalNoAmmo".Translate(),
                        groupable = false,
                        action = delegate()
                        {
                            if (tmpCompThingAbility.ConsumeAmmo())
                            {
                                Hediff injectedCrystal = new Hediff();
                                if (tmpCompThingAbility.AmmoDef == RWBYDefOf.FireDustCrystal)
                                {
                                    injectedCrystal = HediffMaker.MakeHediff(RWBYDefOf.RWBY_InjectedFireCrystal, pawn);
                                }
                                else if (tmpCompThingAbility.AmmoDef == RWBYDefOf.IceDustCrystal)
                                {
                                    injectedCrystal = HediffMaker.MakeHediff(RWBYDefOf.RWBY_InjectedIceCrystal, pawn);
                                }
                                else if (tmpCompThingAbility.AmmoDef == RWBYDefOf.LightningDustCrystal)
                                {
                                    injectedCrystal = HediffMaker.MakeHediff(RWBYDefOf.RWBY_InjectedLightningCrystal, pawn);
                                }
                                else if (tmpCompThingAbility.AmmoDef == RWBYDefOf.GravityDustCrystal)
                                {
                                    injectedCrystal = HediffMaker.MakeHediff(RWBYDefOf.RWBY_InjectedGravityCrystal, pawn);
                                }
                                else if (tmpCompThingAbility.AmmoDef == RWBYDefOf.HardLightDustCrystal)
                                {
                                    injectedCrystal = HediffMaker.MakeHediff(RWBYDefOf.RWBY_InjectedHardLightCrystal, pawn);
                                }
                                pawn.health.AddHediff(injectedCrystal);
                                RWBYDefOf.AuraFlicker.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                            }
                        }
                    };
                }
            }
        }

        public virtual Color GetColor()
        {
            return pawn.story.favoriteColor == null ? pawn.story.HairColor : (Color)pawn.story.favoriteColor;
        }

        public virtual string GetLabelColor()
        {
            return GetColor().grayscale > 0.7f ? "<color=#000000>" : "<color=#FFFFFF>";
        }

        public void Draw()
        {
            if (Active())
            {
                if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_RubyDashForm)) return;
                if (bubbleMat == null)
                {
                    bubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent, GetColor());
                }
                float num = Mathf.Lerp(1.2f, 1.55f, CurrentEnergy / maxEnergy);
                Vector3 vector = pawn.Drawer.DrawPos;
                vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
                int num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
                if (num2 < 8)
                {
                    float num3 = (8 - num2) / 8f * 0.05f;
                    vector += impactAngleVect * num3;
                    num -= num3;
                }
                float angle = Rand.Range(0, 360);
                Vector3 s = new Vector3(num, 1f, num);
                Matrix4x4 matrix = default;
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, bubbleMat, 0);
            }
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look<float>(ref maxEnergy, "maxEnergy", 100, false);
            Scribe_Values.Look<float>(ref currentEnergy, "currentEnergy", 0, false);
            Scribe_Values.Look<int>(ref lastAbsorbDamageTick, "lastAbsorbDamageTick", -9999, false);
            Scribe_References.Look<Pawn>(ref pawn, "auraOwner", false);
        }

        public float maxEnergy;
        private float currentEnergy;
        public int lastAbsorbDamageTick = -9999;
        public Pawn pawn;
        private Vector3 impactAngleVect;
        private Material bubbleMat;
    }
}
