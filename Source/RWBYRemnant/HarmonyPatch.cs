using HarmonyLib;
using Verse;

namespace RWBYRemnant
{
    [StaticConstructorOnStartup]
    static class HarmonyPatch
    {
        static HarmonyPatch()
        {
            var harmony = new Harmony("rimworld.carnysenpai.rwbyremnant");
            // TODO add patches
        }
    }
}
