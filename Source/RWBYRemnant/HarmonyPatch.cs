using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    [StaticConstructorOnStartup]
    static class HarmonyPatch
    {
        static HarmonyPatch()
        {
            var harmony = new Harmony("rimworld.carnysenpai.rwbyremnant");
            harmony.Patch(AccessTools.Method(typeof(Pawn_EquipmentTracker), "GetGizmos"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("GetGizmos_PostFix")), null); // adds thing abilities to pawns
            harmony.Patch(AccessTools.Method(typeof(GenDrop), "TryDropSpawn"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("TryDropSpawn_PostFix")), null); // lets light copies disappear on drop
            harmony.Patch(AccessTools.Method(typeof(Verb_MeleeAttackDamage), "DamageInfosToApply"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("DamageInfosToApply_PostFix")), null); // strenghtens certain pawns melee attacks
            // TODO add patches
        }

        [HarmonyPostfix]
        public static void GetGizmos_PostFix(Pawn_EquipmentTracker __instance, ref IEnumerable<Gizmo> __result) // adds thing abilities to pawns
        {
            Pawn pawn = __instance.pawn;
            if (!pawn.IsColonist) return;
            if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
            {
                List<Gizmo> newOutput = new List<Gizmo>();
                newOutput.AddRange(__result);

                foreach (ThingWithComps thingWithComps in __instance.AllEquipmentListForReading)
                {
                    foreach (ThingComp thingComp in thingWithComps.AllComps.FindAll(c => c is CompCameraPhotos || c is CompWeaponDrinkCoffee || (c is CompStealAura && pawn.Drafted)))
                    {
                        newOutput.AddRange(thingComp.CompGetGizmosExtra());
                    }
                }
                if (!pawn.Drafted)
                {
                    if (pawn.equipment.Primary != null && pawn.equipment.Primary.TryGetComp<CompLightCopy>() != null)
                    {
                        pawn.equipment.Primary.Destroy();
                    }
                }
                __result = newOutput;
            }
        }

        [HarmonyPostfix]
        public static void TryDropSpawn_PostFix(Thing thing) // lets light copies disappear on drop
        {
            if (thing != null && thing.GetType().Equals(typeof(ThingWithComps)) && ((ThingWithComps)thing).TryGetComp<CompLightCopy>() != null)
            {
                thing.Destroy(DestroyMode.Vanish);
            }
        }

        [HarmonyPostfix]
        public static void DamageInfosToApply_PostFix(Verb_MeleeAttackDamage __instance, ref IEnumerable<DamageInfo> __result, LocalTargetInfo target) // strenghtens certain pawns melee attacks
        {
            if (!__instance.CasterIsPawn) return;
            List<DamageInfo> newOutput = new List<DamageInfo>();
            newOutput.AddRange(__result);

            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_YangReturnDamage)) // add Yang damage
            {
                CompAura compAura = __instance.CasterPawn.TryGetComp<CompAura>();
                if (compAura == null) return;
                Aura_Yang aura_Yang = (Aura_Yang)compAura.aura;
                if (aura_Yang == null) return;
                if (aura_Yang.absorbedDamage == 0f) return;
                float damageAmount = aura_Yang.absorbedDamage;

                DamageInfo extraDinfo = new DamageInfo(DamageDefOf.Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }
            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_LightningBuff)) // add Nora damage
            {
                float damageAmount = 20f;

                DamageInfo extraDinfo = new DamageInfo(RWBYDefOf.RWBY_Lightning_Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }

            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_InjectedFireCrystal)) // add Fire Dust Crystal injection damage
            {
                float damageAmount = 20f + 10f * __instance.CasterPawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_InjectedFireCrystal).CurStageIndex;

                DamageInfo extraDinfo = new DamageInfo(RWBYDefOf.RWBY_Inflame_Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }
            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_InjectedIceCrystal)) // add Ice Dust Crystal injection damage
            {
                float damageAmount = 20f + 10f * __instance.CasterPawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_InjectedIceCrystal).CurStageIndex;

                DamageInfo extraDinfo = new DamageInfo(RWBYDefOf.RWBY_Ice_Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }
            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_InjectedLightningCrystal)) // add Lightning Dust Crystal injection damage
            {
                float damageAmount = 20f + 10f * __instance.CasterPawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_InjectedLightningCrystal).CurStageIndex;

                DamageInfo extraDinfo = new DamageInfo(RWBYDefOf.RWBY_Lightning_Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }
            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_InjectedGravityCrystal)) // add Gravity Dust Crystal injection damage
            {
                float damageAmount = 20f + 10f * __instance.CasterPawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_InjectedGravityCrystal).CurStageIndex;

                DamageInfo extraDinfo = new DamageInfo(DamageDefOf.Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }
            if (__instance.CasterPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_InjectedHardLightCrystal)) // add HardLight Dust Crystal injection damage
            {
                float damageAmount = 20f + 10f * __instance.CasterPawn.health.hediffSet.hediffs.Find(h => h.def == RWBYDefOf.RWBY_InjectedHardLightCrystal).CurStageIndex;

                DamageInfo extraDinfo = new DamageInfo(RWBYDefOf.RWBY_Burn_Blunt, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
                extraDinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                BodyPartGroupDef bodyPartGroupDef = __instance.verbProps.AdjustedLinkedBodyPartsGroup(__instance.tool);
                extraDinfo.SetWeaponBodyPartGroup(bodyPartGroupDef);
                HediffDef hediffDef = null;
                if (__instance.HediffCompSource != null)
                {
                    hediffDef = __instance.HediffCompSource.Def;
                }
                extraDinfo.SetWeaponHediff(hediffDef);
                Vector3 direction = (target.Thing.Position - __instance.CasterPawn.Position).ToVector3();
                extraDinfo.SetAngle(direction);
                newOutput.Add(extraDinfo);
            }

            __result = newOutput;
        }
    }
}
