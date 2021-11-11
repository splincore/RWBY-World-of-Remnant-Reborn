using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RWBYRemnant
{
    public class IncidentWorker_Nuckelavee : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (GenTicks.TicksGame < GenDate.TicksPerDay * LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().earliestNuckelavee) return false;
            return base.CanFireNowSub(parms);
        }

        public override float BaseChanceThisGame
        {
            get
            {
                if (ModsConfig.RoyaltyActive && this.def.baseChanceWithRoyalty >= 0f)
                {
                    return this.def.baseChanceWithRoyalty * MoodChance();
                }
                return this.def.baseChance * MoodChance();
            }
        }

        public float MoodChance()
        {
            float mood = 0f;
            int pawnCount = 0;
            List<Pawn> pawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists;
            foreach (Pawn pawn in pawns)
            {
                mood += pawn.needs.mood.CurLevelPercentage;
                pawnCount++;
            }
            float averageMood = mood / pawnCount;
            float averageMoodInverted = 1 - averageMood;
            return averageMoodInverted;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            Pawn pawn = PawnGenerator.GeneratePawn(RWBYDefOf.Grimm_Nuckelavee, FactionUtility.DefaultFactionFrom(RWBYDefOf.Creatures_of_Grimm));
            if (!parms.spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Hostile, false, null))
            {
                return false;
            }
            parms.spawnRotation = Rot4.FromAngleFlat((map.Center - parms.spawnCenter).AngleFlat);
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
            if (pawn is Pawn_Grimm pawn_Grimm) pawn_Grimm.SetNuckelaveeTimer(Rand.RangeInclusive(30000, 60000));
            GenSpawn.Spawn(pawn, loc, map, parms.spawnRotation, WipeMode.Vanish, false);
            string label = "LetterLabelNuckelavee".Translate();
            string text = "LetterTextNuckelavee".Translate();
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, pawn);
            return true;
        }
    }
}
