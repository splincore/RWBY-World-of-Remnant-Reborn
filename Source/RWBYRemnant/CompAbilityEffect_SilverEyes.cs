using RimWorld;
using Verse;
using Verse.AI;

namespace RWBYRemnant
{
    public class CompAbilityEffect_SilverEyes : CompAbilityEffect
    {
        public Pawn CasterPawn => parent.pawn;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(CasterPawn.DrawPos + CasterPawn.Drawer.renderer.BaseHeadOffsetAt(CasterPawn.Rotation), CasterPawn.Map, RWBYDefOf.RWBY_SilverEye_Fleck);
            dataStatic.rotationRate = 0f;
            dataStatic.velocityAngle = 0f;
            dataStatic.velocitySpeed = 0f;
            CasterPawn.Map.flecks.CreateFleck(dataStatic);

            foreach (Pawn grimm in CasterPawn.Map.mapPawns.AllPawnsSpawned.FindAll(g => g.Faction != null && g.Faction.def == RWBYDefOf.Creatures_of_Grimm && GrimmUtility.IsGrimm(g)))
            {
                float damageToDeal = 75f * CasterPawn.health.hediffSet.hediffs.FindAll(h => h.def == RWBYDefOf.RWBY_SilverEyes).Count;
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, damageToDeal, instigator: CasterPawn);
                if (grimm.CanSee(CasterPawn))
                {
                    grimm.stances.stunner.StunFor(GenTicks.SecondsToTicks(4.5f), CasterPawn);
                    grimm.TakeDamage(dinfo);
                }
            }

            Hediff silverEyesExhaustion = HediffMaker.MakeHediff(RWBYDefOf.RWBY_SilverEye_Exhaustion, CasterPawn);
            if (CasterPawn.story.traits.HasTrait(RWBYDefOf.RWBY_Aura))
            {
                silverEyesExhaustion.Severity = 2f;
            }
            else
            {
                silverEyesExhaustion.Severity = 1f;
            }
            CasterPawn.health.AddHediff(silverEyesExhaustion);

            base.Apply(target, dest);
        }
    }
}
