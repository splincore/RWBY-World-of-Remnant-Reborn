﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace RWBYRemnant
{
    public class Pawn_Grimm : Pawn
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (attractGrimmTimer <= 0 && nuckelaveeTimer <= 0 && this.RaceProps.AnyPawnKind != RWBYDefOf.Grimm_Apathy)
            {
                TriggerAggro();
            }
        }

        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostApplyDamage(dinfo, totalDamageDealt);
            lastTakenDamageFrom = dinfo.Instigator;
            if (!this.Dead)
            {
                if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Nuckelavee && !this.InMentalState) Messages.Message("MessageTextNuckelaveeTriggered".Translate().CapitalizeFirst(), this, MessageTypeDefOf.NegativeEvent);
                TriggerAggro();
            }
        }

        public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            if (lastTakenDamageFrom is Pawn killer && killer.RaceProps.Humanlike && killer.story.traits.HasTrait(RWBYDefOf.Semblance_Weiss) && !killer.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen))
            {
                if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Boarbatusk)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonBoar);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Beowolf)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonBeowolf);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Ursa)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonUrsa);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Griffon)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonGriffon);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Nevermore)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonNevermore);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Lancer)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonLancer);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_LancerQueen)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonLancerQueen);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_DeathStalker)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonDeathStalker);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Nuckelavee)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonNuckelavee);
                }
                else if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Apathy)
                {
                    killer.abilities.GainAbility(RWBYDefOf.Weiss_SummonApathy);
                }
            }
            base.Kill(dinfo, exactCulprit);
        }

        public override void Tick()
        {
            base.Tick();
            if (Downed)
            {
                Kill(null);
            }
            attractGrimmTimer--;
            nuckelaveeTimer--;

            if (this.Map != null && this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Nuckelavee && nuckelaveeTimer > 0 && nuckelaveeTimer % 2500 == 0 && !this.InMentalState) // Nuckelavee spawn more Grimm
            {
                Pawn pawn = PawnGenerator.GeneratePawn(GrimmUtility.GetRandomGrimmBalanced(), FactionUtility.DefaultFactionFrom(RWBYDefOf.Creatures_of_Grimm));
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(this.Position, this.Map, 8, null);
                if (pawn is Pawn_Grimm pawn_Grimm) pawn_Grimm.SetNuckelaveeTimer(nuckelaveeTimer);
                GenSpawn.Spawn(pawn, loc, this.Map, this.Rotation, WipeMode.Vanish, false);
            }

            if (this.Map != null && this.IsHashIntervalTick(30000) && this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Apathy && !this.InMentalState && this.Map.mapPawns.AllPawnsSpawned.FindAll(p => p.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Apathy).Count() < this.Map.mapPawns.ColonistCount * 10) // Apathy spawn more Apathy
            {
                Pawn pawn = PawnGenerator.GeneratePawn(RWBYDefOf.Grimm_Apathy, FactionUtility.DefaultFactionFrom(RWBYDefOf.Creatures_of_Grimm));
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(this.Position, this.Map, 8, null);
                if (pawn is Pawn_Grimm pawn_Grimm) pawn_Grimm.SetNuckelaveeTimer(nuckelaveeTimer);
                GenSpawn.Spawn(pawn, loc, this.Map, this.Rotation, WipeMode.Vanish, false);
            }

            if (nuckelaveeTimer == 0)
            {
                if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Nuckelavee && !this.InMentalState) Messages.Message("MessageTextNuckelaveeTriggered".Translate().CapitalizeFirst(), this, MessageTypeDefOf.NegativeEvent);
                TriggerAggro();
            }

            if (attractGrimmTimer == 0)
            {
                IncidentDef localDef = IncidentDefOf.RaidEnemy;
                IncidentParms parms = StorytellerUtility.DefaultParmsNow(localDef.category, Map);
                StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
                parms = storytellerComp.GenerateParms(localDef.category, parms.target);
                parms.faction = FactionUtility.DefaultFactionFrom(RWBYDefOf.Creatures_of_Grimm);
                parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
                localDef.Worker.TryExecute(parms);
                TriggerAggro();
            }
        }

        public override void TickRare()
        {
            base.TickRare();
            if (Map != null && Map.mapPawns.AllPawnsSpawned.Any(p => p.RaceProps.Humanlike && !p.health.hediffSet.HasHediff(RWBYDefOf.RWBY_MaskedEmotions) && p.Position.DistanceTo(this.Position) < 20))
            {
                TriggerAggro();
            }
            if (this.CurJob == null) return;
            if (this.CurJob.targetA.Thing is Pawn pawnA && pawnA.health.hediffSet.HasHediff(RWBYDefOf.RWBY_MaskedEmotions))
            {
                jobs.EndCurrentJob(JobCondition.InterruptForced, true);
            }
            if (this.CurJob.targetB.Thing is Pawn pawnB && pawnB.health.hediffSet.HasHediff(RWBYDefOf.RWBY_MaskedEmotions))
            {
                jobs.EndCurrentJob(JobCondition.InterruptForced, true);
            }
            if (this.CurJob.targetC.Thing is Pawn pawnC && pawnC.health.hediffSet.HasHediff(RWBYDefOf.RWBY_MaskedEmotions))
            {
                jobs.EndCurrentJob(JobCondition.InterruptForced, true);
            }
        }

        public void SetAttractGrimmTimer()
        {
            attractGrimmTimer = Rand.RangeInclusive(5000, 7500); // 2-3 ingame hours
            this.SetFaction(null);
        }

        public void SetNuckelaveeTimer(int timer)
        {
            nuckelaveeTimer = timer;
            this.SetFaction(null);
        }

        public void TriggerAggro()
        {
            if (triggeredAggro) return; // only try once, because permanent
            if (this.Faction == null) this.SetFaction(FactionUtility.DefaultFactionFrom(RWBYDefOf.Creatures_of_Grimm));
            if (!this.InMentalState) this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, true, false, null, true);
            triggeredAggro = true;
            if (this.RaceProps.AnyPawnKind == RWBYDefOf.Grimm_Apathy && this.Map != null)
            {
                List<Pawn> pawns = this.Map.mapPawns.AllPawnsSpawned.FindAll(p => p is Pawn_Grimm && !p.InMentalState);
                foreach (Pawn_Grimm pawn in pawns.Cast<Pawn_Grimm>())
                {
                    pawn.TriggerAggro();
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref attractGrimmTimer, "attractGrimmTimer", -9999, false);
            Scribe_Values.Look<int>(ref nuckelaveeTimer, "nuckelaveeTimer", -9999, false);
            Scribe_Values.Look<bool>(ref triggeredAggro, "triggeredAggro", false, false);
            Scribe_References.Look<Thing>(ref lastTakenDamageFrom, "lastTakenDamageFrom", false);
        }

        private int attractGrimmTimer = -9999;
        private int nuckelaveeTimer = -9999;
        private bool triggeredAggro = false;
        private Thing lastTakenDamageFrom = null;
    }
}