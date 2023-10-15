using RimWorld;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_PyrrhaUseMagnetism : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                if (target.Thing is Thing thing && SemblanceUtility.PyrrhaMagnetismCanAffect(thing))
                {
                    int countToPickup = MassUtility.CountToPickUpUntilOverEncumbered(parent.pawn, thing);
                    if (countToPickup <= 0) return;
                    parent.pawn.inventory.GetDirectlyHeldThings().TryAddOrTransfer(thing.SplitOff(Mathf.Min(countToPickup, thing.stackCount)));
                }
                base.Apply(target, dest);
            }
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return base.Valid(target, throwMessages) && target.Thing is Thing thing && SemblanceUtility.PyrrhaMagnetismCanAffect(thing) && MassUtility.CountToPickUpUntilOverEncumbered(parent.pawn, thing) > 0;
        }
    }
}
