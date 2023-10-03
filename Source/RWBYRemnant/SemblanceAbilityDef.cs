using RimWorld;

namespace RWBYRemnant
{
    public class SemblanceAbilityDef : AbilityDef
    {
        public float AuraCost
        {
            get
            {
                return this.statBases.GetStatValueFromList(RWBYDefOf.AuraCost, 0f);
            }
        }
    }
}
