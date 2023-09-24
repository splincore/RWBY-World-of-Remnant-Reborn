using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using EquipmentToolbox;

namespace RWBYRemnant
{
    public class Aura_Blake : Aura
    {
        public override bool TryAbsorbDamage(DamageInfo dinfo)
        {
            if (dinfo.Def.defName == "PJ_ForceHealDamage") return base.TryAbsorbDamage(dinfo);
            if (dinfo.Instigator != null && dinfo.Instigator.def == RWBYDefOf.Blake_ShadowClone_Fire) return true;
            if ((pawn.Drafted || pawn.IsFighting()) && Rand.Chance(0.5f))
            {
                Building edifice = pawn.Position.GetEdifice(pawn.Map);
                if ((edifice == null || edifice.def.Fillage == FillCategory.None) && TryConsumeAura(0.02f))
                {
                    UseShadowClone(dinfo);
                    return true;
                }
            }
            return base.TryAbsorbDamage(dinfo);
        }

        public void UseShadowClone(DamageInfo dinfo)
        {
            ThingDef cloneToUse = RWBYDefOf.Blake_ShadowClone; // default
            float cloneDuration = 2f;
            Color color = new Color(1.0f, 1.0f, 1.0f);
            if (elementalShadowCloneToUse != null)
            {
                bool canUseElementalClone = false;
                if (pawn.apparel.WornApparel.Find(b => b.def == RWBYDefOf.Apparel_PowderedDustBelt) is ThingWithComps belt)
                {
                    if (elementalShadowCloneToUse == RWBYDefOf.Blake_ShadowClone_Fire)
                    {
                        if (belt.AllComps.Find(c => c is CompThingAbility tmpCompThingAbility && tmpCompThingAbility.AmmoDef == RWBYDefOf.FireDustPowder) is CompThingAbility compThingAbility && compThingAbility.ConsumeAmmo())
                        {
                            canUseElementalClone = true;
                        }
                    }
                    if (elementalShadowCloneToUse == RWBYDefOf.Blake_ShadowClone_Ice)
                    {
                        if (belt.AllComps.Find(c => c is CompThingAbility tmpCompThingAbility && tmpCompThingAbility.AmmoDef == RWBYDefOf.IceDustPowder) is CompThingAbility compThingAbility && compThingAbility.ConsumeAmmo())
                        {
                            canUseElementalClone = true;
                        }
                    }
                    if (elementalShadowCloneToUse == RWBYDefOf.Blake_ShadowClone_Stone)
                    {
                        if (belt.AllComps.Find(c => c is CompThingAbility tmpCompThingAbility && tmpCompThingAbility.AmmoDef == RWBYDefOf.GravityDustPowder) is CompThingAbility compThingAbility && compThingAbility.ConsumeAmmo())
                        {
                            canUseElementalClone = true;
                        }
                    }
                }
                if (canUseElementalClone)
                {
                    cloneToUse = elementalShadowCloneToUse;
                    cloneDuration = elementalShadowCloneDuration;
                    color = elementalShadowCloneColor;
                }
            }
            if (cloneToUse == RWBYDefOf.Blake_ShadowClone_Ice && dinfo.Instigator is Pawn attackerPawn && dinfo.Weapon != null && !dinfo.Weapon.IsRangedWeapon) // add stun(freeze) to melee attacker
            {
                attackerPawn.stances.stunner.StunFor(GenTicks.SecondsToTicks(4.5f), pawn);
            }
            Thing thing = ThingMaker.MakeThing(cloneToUse);
            thing.TryGetComp<CompShadowClone>().SetData(pawn, GenTicks.SecondsToTicks(cloneDuration), color);
            GenSpawn.Spawn(thing, pawn.Position, pawn.Map);
            thing.TryGetComp<CompShadowClone>().MovePawnOut();
            if (cloneToUse == RWBYDefOf.Blake_ShadowClone_Fire) thing.TryGetComp<CompExplosiveSilent>().StartWick();
        }

        public void SetClone(ThingDef thingDef, float cloneDuration, Color color)
        {
            elementalShadowCloneToUse = thingDef;
            elementalShadowCloneDuration = cloneDuration;
            elementalShadowCloneColor = color;
        }

        public override Color GetColor()
        {
            return color;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            Command_Toggle useNoClone = new Command_Toggle
            {
                defaultLabel = "BlakeUseDefaultCloneLabel".Translate(),
                defaultDesc = "BlakeUseDefaultCloneDescription".Translate(),
                icon = TexturesRWBY.UiDefaultClone,
                disabled = false,
                isActive = () => elementalShadowCloneToUse == null,
                toggleAction = delegate ()
                {
                    SetClone(null, 0, new Color());
                }
            };
            yield return useNoClone;
            Command_Toggle useFireClone = new Command_Toggle
            {
                defaultLabel = "BlakeUseFireCloneLabel".Translate(),
                defaultDesc = "BlakeUseFireCloneDescription".Translate(),
                icon = TexturesRWBY.UiFireDust,
                disabled = false,
                isActive = () => elementalShadowCloneToUse == RWBYDefOf.Blake_ShadowClone_Fire,
                toggleAction = delegate ()
                {
                    SetClone(RWBYDefOf.Blake_ShadowClone_Fire, 3, new Color(1.0f, 0.0f, 0.0f));
                }
            };
            yield return useFireClone;
            Command_Toggle useIceClone = new Command_Toggle
            {
                defaultLabel = "BlakeUseIceCloneLabel".Translate(),
                defaultDesc = "BlakeUseIceCloneDescription".Translate(),
                icon = TexturesRWBY.UiIceDust,
                disabled = false,
                isActive = () => elementalShadowCloneToUse == RWBYDefOf.Blake_ShadowClone_Ice,
                toggleAction = delegate ()
                {
                    SetClone(RWBYDefOf.Blake_ShadowClone_Ice, 4, new Color(0.4f, 0.8f, 1.0f));
                }
            };
            yield return useIceClone;
            Command_Toggle useGravityClone = new Command_Toggle
            {
                defaultLabel = "BlakeUseGravityCloneLabel".Translate(),
                defaultDesc = "BlakeUseGravityCloneDescription".Translate(),
                icon = TexturesRWBY.UiGravityDust,
                disabled = false,
                isActive = () => elementalShadowCloneToUse == RWBYDefOf.Blake_ShadowClone_Stone,
                toggleAction = delegate ()
                {
                    SetClone(RWBYDefOf.Blake_ShadowClone_Stone, 8, new Color(0.1f, 0.1f, 0.1f));
                }
            };
            yield return useGravityClone;

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ThingDef>(ref elementalShadowCloneToUse, "elementalShadowCloneToUse");
            Scribe_Values.Look<float>(ref elementalShadowCloneDuration, "elementalShadowCloneDuration", 2f, false);
            Scribe_Values.Look<Color>(ref elementalShadowCloneColor, "elementalShadowCloneColor", new Color(), false);
        }

        public ThingDef elementalShadowCloneToUse;
        public float elementalShadowCloneDuration;
        public Color elementalShadowCloneColor;
        public Color color = new Color(0.8f, 0.2f, 1.0f);
    }
}
