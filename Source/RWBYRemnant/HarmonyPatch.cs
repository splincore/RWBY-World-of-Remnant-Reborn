using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

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
            harmony.Patch(AccessTools.Method(typeof(Pawn_HealthTracker), "PreApplyDamage"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("PreApplyDamage_PostFix")), null); // aura absorb
            harmony.Patch(AccessTools.Method(typeof(Pawn_HealthTracker), "NotifyPlayerOfKilled"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("PreNotifyPlayerOfKilled_PreFix")), null, null); // disables notification if summoned Grimm disappears
            //harmony.Patch(AccessTools.Method(typeof(Pawn_HealthTracker), "AddHediff", new[] { typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo), typeof(DamageWorker.DamageResult) }), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("AddHediff_PreFix")), null, null);  // makes Nora immune to RimTasers Reloaded debuff and charges her
            harmony.Patch(AccessTools.Method(typeof(Pawn), "DrawAt", new[] { typeof(Vector3), typeof(bool) }), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("RenderPawnAt_PreFix")), null, null); // makes invisible: Ruby while dashing, Apathy while not triggered
            harmony.Patch(AccessTools.Method(typeof(Pawn), "DrawGUIOverlay"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("RenderPawnAt_PreFix")), null, null); // makes invisible: Ruby while dashing, Apathy while not triggered
            //harmony.Patch(AccessTools.Method(typeof(DamageWorker_Flame), "ExplosionAffectCell"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("ExplosionAffectCell_PostFix")), null); // makes fire Dust spawn fire on explosion
            //harmony.Patch(AccessTools.Method(typeof(JobDriver_Wait), "CheckForAutoAttack"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("CheckForAutoAttack_PreFix")), null, null); // fixes summoned Grimm bug of nullpointer if wandering
            //harmony.Patch(AccessTools.Method(typeof(WeatherEvent_LightningStrike), "FireEvent"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("FireEvent_PreFix")), null, null); // changes lightning stike location onto Nora pawns
            //harmony.Patch(AccessTools.Method(typeof(Thing), "Ingested"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("Ingested_PostFix")), null); // checks for Pumpkin Pete´s eaten
            //harmony.Patch(AccessTools.Method(typeof(Pawn_RecordsTracker), "Increment"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("Increment_PostFix")), null); // unlocks Semblance Shooting Melee Construction Mining Cooking Plants Animals Medicine
            //harmony.Patch(AccessTools.Method(typeof(Pawn_JobTracker), "StartJob"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("StartJob_PostFix")), null); // unlocks Semblance Intellectual
            //harmony.Patch(AccessTools.Method(typeof(RecordsUtility), "Notify_BillDone"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("Notify_BillDone_PostFix")), null); // unlocks Semblance Crafting Artistic
            //harmony.Patch(AccessTools.Method(typeof(Trait), "TipString"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("TipString_PostFix")), null); // adds disabled working tags to Trait descriptions
            //harmony.Patch(AccessTools.Method(typeof(GenHostility), "HostileTo", new[] { typeof(Thing), typeof(Thing) }), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("HostileTo_PostFix")), null); // makes Grimm unable to attack pawns affected by Ren or without negative emotions
            //harmony.Patch(AccessTools.Method(typeof(IncidentWorker_RaidEnemy), "TryExecuteWorker"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("TryExecuteWorker_PreFix")), null, null);  // may increases raid size if Semblance Qrow is present
            //harmony.Patch(AccessTools.Method(typeof(AttackTargetFinder), "BestAttackTarget"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("BestAttackTarget_PreFix")), null, null); // makes Grimm not need line of sight
            //harmony.Patch(AccessTools.Method(typeof(Pawn_InteractionsTracker), "TryInteractWith"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("TryInteractWith_PostFix")), null); // unlock Semblance Social
            //harmony.Patch(AccessTools.Method(typeof(Targeter), "ProcessInputEvents"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("ProcessInputEvents_Prefix")), null, null); // lets the weapon projectile ability aim properly
            //harmony.Patch(AccessTools.Method(typeof(PawnGenerator), "GeneratePawn", new[] { typeof(PawnGenerationRequest) }), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("GeneratePawn_PostFix")), null); // adds silver eyes to a humanoid pawn // TODO add silver eyes
            harmony.Patch(AccessTools.Method(typeof(AbilityDef), "StatSummary"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("StatSummary_PostFix")), null); // add Ability Tooltip Aura cost stat
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
            if (!__instance.CasterIsPawn || __instance.CasterPawn.NonHumanlikeOrWildMan()) return;
            List<DamageInfo> newOutput = new List<DamageInfo>();
            newOutput.AddRange(__result);

            foreach ((HediffDef statusType, DamageDef damageType) in SemblanceUtility.damageBoostingHediffs)
            {
                if (__instance.CasterPawn.health.hediffSet.HasHediff(statusType))
                {
                    float damageAmount = 20f + 10f * __instance.CasterPawn.health.hediffSet.hediffs.Find(h => h.def == statusType).CurStageIndex;
                    if (statusType == RWBYDefOf.RWBY_YangReturnDamage)
                    {
                        CompAura compAura = __instance.CasterPawn.TryGetComp<CompAura>();
                        if (compAura == null) return;
                        Aura_Yang aura_Yang = (Aura_Yang)compAura.aura;
                        if (aura_Yang == null) return;
                        if (aura_Yang.absorbedDamage == 0f) return;
                        damageAmount = aura_Yang.absorbedDamage;
                    }
                    DamageInfo extraDinfo = new DamageInfo(damageType, damageAmount, 0f, -1f, __instance.caster, null, __instance.CasterPawn.def, DamageInfo.SourceCategory.ThingOrUnknown, null);
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
            }
            __result = newOutput;
        }

        [HarmonyPostfix]
        public static void PreApplyDamage_PostFix(Pawn ___pawn, DamageInfo dinfo, ref bool absorbed) // aura absorb
        {
            if (dinfo.Def == DamageDefOf.Extinguish)
            {
                return;
            }
            if (___pawn.CurJobDef == RWBYDefOf.RWBY_StealAura) ___pawn.jobs.EndCurrentJob(JobCondition.InterruptForced); // makes the Aura steal interruptable
            if (!absorbed && ___pawn.TryGetComp<CompAura>() != null && ___pawn.TryGetComp<CompAura>().aura != null)
            {
                if (___pawn.GetComp<CompAura>().aura.TryAbsorbDamage(dinfo))
                {
                    absorbed = true;
                }
            }
        }

        [HarmonyPrefix]
        public static bool PreNotifyPlayerOfKilled_PreFix(Pawn ___pawn) // disables notification if summoned Grimm disappears
        {
            if (___pawn is Pawn_SummonedGrimm) return false;
            return true;
        }

        [HarmonyPrefix]
        public static bool RenderPawnAt_PreFix(Pawn __instance) // makes invisible: Ruby while dashing, Apathy while not triggered
        {
            if (__instance.health.hediffSet.HasHediff(RWBYDefOf.RWBY_RubyDashForm)) return false;
            if (__instance.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Apathy && !__instance.InMentalState) return false;
            return true;
        }

        [HarmonyPostfix]
        public static void StatSummary_PostFix(AbilityDef __instance, ref IEnumerable<string> __result) // add Ability Tooltip Aura cost stat
        {
            if (__instance is SemblanceAbilityDef semblanceAbilityDef)
            {
                List<string> result = new List<string>();
                result.AddRange(__result);
                result.Add("SemblanceAuraCost".Translate() + ": " + semblanceAbilityDef.AuraCost);
                __result = result;
            }
        }
    }
}
