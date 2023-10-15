using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_CinderSummonWeapon : CompAbilityEffect
    {
        public new CompProperties_CinderSummonWeapon Props => (CompProperties_CinderSummonWeapon)props;
        public Pawn CasterPawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Thing == CasterPawn)
            {
                ThingWithComps createdWeapon = (ThingWithComps)ThingMaker.MakeThing(Props.weaponToSummon);
                createdWeapon.TryGetComp<CompQuality>().SetQuality(QualityCategory.Masterwork, ArtGenerationContext.Colony);
                ThingWithComps currentWeapon = CasterPawn.equipment.Primary;
                if (currentWeapon != null)
                {
                    if (currentWeapon.TryGetComp<CompLightCopy>() != null)
                    {
                        currentWeapon.Destroy();
                    }
                    else
                    {
                        CasterPawn.inventory.GetDirectlyHeldThings().TryAddOrTransfer(currentWeapon);
                    }
                }
                CasterPawn.equipment.AddEquipment(createdWeapon);
            }
            base.Apply(target, dest);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return base.Valid(target, throwMessages) && target.Thing == CasterPawn;
        }
    }
}
