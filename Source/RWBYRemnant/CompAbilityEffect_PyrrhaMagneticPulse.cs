using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_PyrrhaMagneticPulse : CompAbilityEffect
    {
        Pawn CasterPawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            foreach (Thing thing in CasterPawn.Map.spawnedThings.Where<Thing>(t => t.Position.DistanceTo(CasterPawn.Position) <= parent.def.EffectRadius && SemblanceUtility.PyrrhaMagnetismCanAffect(t)))
            {
                if (thing is Pawn pawn && !pawn.HostileTo(CasterPawn))
                {
                    Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(RWBYDefOf.RWBY_PyrrhaHurtFriendly);
                    pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory);
                }
                DamageInfo dinfo1 = new DamageInfo(DamageDefOf.EMP, 1f, instigator: CasterPawn);
                dinfo1.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                Vector3 direction1 = (thing.Position - CasterPawn.Position).ToVector3();
                dinfo1.SetAngle(direction1);
                thing.TakeDamage(dinfo1);

                DamageInfo dinfo2 = new DamageInfo(DamageDefOf.Blunt, 200f, instigator: CasterPawn);
                dinfo2.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
                Vector3 direction2 = (thing.Position - CasterPawn.Position).ToVector3();
                dinfo2.SetAngle(direction2);
                thing.TakeDamage(dinfo2);
            }
            base.Apply(target, dest);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return base.Valid(target, throwMessages) && target.Thing == CasterPawn;
        }
    }
}
