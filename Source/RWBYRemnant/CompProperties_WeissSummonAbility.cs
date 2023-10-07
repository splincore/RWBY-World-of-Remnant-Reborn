using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompProperties_WeissSummonAbility : CompProperties_AbilityEffect
    {
        public CompProperties_WeissSummonAbility()
        {
            compClass = typeof(CompAbilityEffect_WeissSummon);
        }

        public PawnKindDef summonToSpawn;
        public ThingDef thingToSpawn;
        public float timeToSummon;
    }
}
