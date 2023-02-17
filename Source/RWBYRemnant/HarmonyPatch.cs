﻿using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
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
                    foreach (ThingComp thingComp in thingWithComps.AllComps.FindAll(c => c is CompCameraPhotos || c is CompWeaponDrinkCoffee || (c is CompStealAura && pawn.Drafted))) // TODO add c is CompWeaponDrinkCoffee
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
    }
}
