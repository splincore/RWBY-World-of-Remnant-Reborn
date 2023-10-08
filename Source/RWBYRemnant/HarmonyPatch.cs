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
            harmony.Patch(AccessTools.Method(typeof(Pawn), "DrawAt", new[] { typeof(Vector3), typeof(bool) }), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("RenderPawnAt_PreFix")), null, null); // makes invisible: Ruby while dashing, Apathy while not triggered
            harmony.Patch(AccessTools.Method(typeof(Pawn), "DrawGUIOverlay"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("RenderPawnAt_PreFix")), null, null); // makes invisible: Ruby while dashing, Apathy while not triggered
            harmony.Patch(AccessTools.Method(typeof(DamageWorker_Flame), "ExplosionAffectCell"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("ExplosionAffectCell_PostFix")), null); // makes fire Dust spawn fire on explosion
            //harmony.Patch(AccessTools.Method(typeof(JobDriver_Wait), "CheckForAutoAttack"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("CheckForAutoAttack_PreFix")), null, null); // fixes summoned Grimm bug of nullpointer if wandering
            harmony.Patch(AccessTools.Method(typeof(WeatherEvent_LightningStrike), "FireEvent"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("FireEvent_PreFix")), null, null); // changes lightning stike location onto Nora pawns
            harmony.Patch(AccessTools.Method(typeof(Thing), "Ingested"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("Ingested_PostFix")), null); // checks for Pumpkin Pete´s eaten
            //harmony.Patch(AccessTools.Method(typeof(Pawn_RecordsTracker), "Increment"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("Increment_PostFix")), null); // unlocks Semblance Shooting Melee Construction Mining Cooking Plants Animals Medicine
            //harmony.Patch(AccessTools.Method(typeof(Pawn_JobTracker), "StartJob"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("StartJob_PostFix")), null); // unlocks Semblance Intellectual
            //harmony.Patch(AccessTools.Method(typeof(RecordsUtility), "Notify_BillDone"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("Notify_BillDone_PostFix")), null); // unlocks Semblance Crafting Artistic
            //harmony.Patch(AccessTools.Method(typeof(Pawn_InteractionsTracker), "TryInteractWith"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("TryInteractWith_PostFix")), null); // unlock Semblance Social
            harmony.Patch(AccessTools.Method(typeof(GenHostility), "HostileTo", new[] { typeof(Thing), typeof(Thing) }), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("HostileTo_PostFix")), null); // makes Grimm unable to attack pawns affected by Ren or without negative emotions
            harmony.Patch(AccessTools.Method(typeof(AttackTargetFinder), "BestAttackTarget"), new HarmonyMethod(typeof(HarmonyPatch).GetMethod("BestAttackTarget_PreFix")), null, null); // makes Grimm not need line of sight
            harmony.Patch(AccessTools.Method(typeof(AbilityDef), "StatSummary"), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("StatSummary_PostFix")), null); // add Ability Tooltip Aura cost stat
            harmony.Patch(AccessTools.Method(typeof(PawnGenerator), "GeneratePawn", new[] { typeof(PawnGenerationRequest) }), null, new HarmonyMethod(typeof(HarmonyPatch).GetMethod("GeneratePawn_PostFix")), null); // adds silver eyes to a humanoid pawn
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
        public static void Ingested_PostFix(ref float __result, Thing __instance, Pawn ingester) // checks for Pumpkin Pete´s eaten
        {
            if (__instance.def == RWBYDefOf.RWBY_PumpkinPetes && __result != 0)
            {
                if (ingester.TryGetComp<CompAura>() is CompAura compAura)
                {
                    compAura.Notify_EatenPumkinPetes();

                    if (ingester.story != null && ingester.story.traits.HasTrait(RWBYDefOf.Semblance_Jaune))
                    {
                        Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(RWBYDefOf.RWBY_JauneAtePumpkinPetes);
                        ingester.needs.mood.thoughts.memories.TryGainMemory(thought_Memory);
                    }
                }
            }
        }

        [HarmonyPostfix]
        public static void ExplosionAffectCell_PostFix(DamageWorker_Flame __instance, Explosion explosion, IntVec3 c, List<Thing> damagedThings, bool canThrowMotes) // makes fire Dust spawn fire on explosion
        {
            if (__instance.def == RWBYDefOf.Bomb_Fire && Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map)))
            {
                FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
            }
        }

        [HarmonyPrefix]
        public static void FireEvent_PreFix(Map ___map, ref IntVec3 ___strikeLoc) // changes lightning stike location onto Nora pawns
        {
            List<Pawn> pawns = ___map.mapPawns.AllPawns.ToList().FindAll(p => p.RaceProps.Humanlike && p.story.traits.HasTrait(RWBYDefOf.Semblance_Nora) && !p.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen));
            if (pawns.Count() > 0)
            {
                IntVec3 strikeLoc = ___strikeLoc;
                pawns = pawns.FindAll(p => p.Position.DistanceTo(strikeLoc) <= 30f && !p.Position.Roofed(___map));
                if (pawns.Count() > 0)
                {
                    Pawn pawn = pawns.RandomElement();
                    ___strikeLoc = pawn.Position;
                    if (pawn.TryGetComp<CompAura>().aura is Aura_Nora aura_Nora) aura_Nora.Notify_NextDamageIsLightning();
                }
            }
        }

        [HarmonyPostfix]
        public static void HostileTo_PostFix(ref bool __result, Thing a, Thing b) // makes Grimm unable to attack pawns affected by Ren or without negative emotions
        {
            if (!__result) return;
            if (!(a is Pawn searcherPawn)) return;
            if (!GrimmUtility.IsGrimm(searcherPawn) || searcherPawn.Faction.def != RWBYDefOf.Creatures_of_Grimm) return;
            if (b is Pawn targetPawn && (targetPawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_MaskedEmotions) || targetPawn.Downed))
            {
                __result = false;
                return;
            }
            if (b is Pawn targetPawn2 && targetPawn2.RaceProps.Humanlike)
            {
                List<Thought> outThoughts = new List<Thought>();
                targetPawn2.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                if (!outThoughts.Any(o => o.MoodOffset() < 0f)) __result = false;
            }
        }

        [HarmonyPrefix]
        public static void BestAttackTarget_PreFix(IAttackTargetSearcher searcher, ref TargetScanFlags flags) // makes Grimm not need line of sight
        {
            if (!(searcher is Pawn searcherPawn)) return;
            if (searcherPawn.Faction == null) return;
            if (!GrimmUtility.IsGrimm(searcherPawn) || searcherPawn.Faction.def != RWBYDefOf.Creatures_of_Grimm) return;
            flags = TargetScanFlags.None;
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

        [HarmonyPostfix]
        public static void GeneratePawn_PostFix(Pawn __result) // adds silver eyes to a humanoid pawn
        {
            if (__result.RaceProps.Humanlike)
            {
                if (SemblanceUtility.PyrrhaMagnetismCanAffect(__result)) return; // removes Androids from birth effects
                if (__result.relations.RelatedPawns.Any(p => p.relations.Children.Contains(__result) && p.health.hediffSet.HasHediff(RWBYDefOf.RWBY_SilverEyes)) || __result.relations.Children.Any(c => c.health.hediffSet.HasHediff(RWBYDefOf.RWBY_SilverEyes)))
                {
                    if (Rand.Chance(0.5f)) // inherit with 50% chance
                    {
                        foreach (BodyPartRecord bodyPartRecord in __result.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Eye))
                        {
                            __result.health.AddHediff(RWBYDefOf.RWBY_SilverEyes, bodyPartRecord);
                            __result.abilities.GainAbility(RWBYDefOf.Ability_SilverEyes);
                        }
                    }
                }
                else if (Rand.Chance(0.01f)) // natural with 1% chance
                {
                    foreach (BodyPartRecord bodyPartRecord in __result.RaceProps.body.GetPartsWithDef(BodyPartDefOf.Eye))
                    {
                        __result.health.AddHediff(RWBYDefOf.RWBY_SilverEyes, bodyPartRecord);
                        __result.abilities.GainAbility(RWBYDefOf.Ability_SilverEyes);
                    }
                }
            }
        }
    }
}
