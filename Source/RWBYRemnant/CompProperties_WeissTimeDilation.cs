using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompProperties_WeissTimeDilation : CompProperties_AbilityEffect
    {
        public CompProperties_WeissTimeDilation()
        {
            compClass = typeof(CompAbilityEffect_WeissTimeDilation);
        }

        public ThingDef thingToSpawn;
    }
}
