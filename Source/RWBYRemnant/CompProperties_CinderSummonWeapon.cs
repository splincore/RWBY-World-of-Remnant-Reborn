using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompProperties_CinderSummonWeapon : CompProperties_AbilityEffect
    {
        public CompProperties_CinderSummonWeapon()
        {
            compClass = typeof(CompAbilityEffect_CinderSummonWeapon);
        }

        public ThingDef weaponToSummon;
    }
}
