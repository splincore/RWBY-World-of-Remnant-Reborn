﻿using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class IncidentWorker_GrimmWandersIn : IncidentWorker_Nuckelavee
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            Pawn pawn = PawnGenerator.GeneratePawn(GrimmUtility.GetRandomGrimmBalanced(), FactionUtility.DefaultFactionFrom(RWBYDefOf.Creatures_of_Grimm));
            if (!parms.spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Hostile, false, null))
            {
                return false;
            }
            parms.spawnRotation = Rot4.FromAngleFlat((map.Center - parms.spawnCenter).AngleFlat);
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
            if (pawn is Pawn_Grimm pawn_Grimm) pawn_Grimm.SetAttractGrimmTimer();
            GenSpawn.Spawn(pawn, loc, map, parms.spawnRotation, WipeMode.Vanish, false);
            string label = "LetterLabelGrimmWandersIn".Translate();
            string text = "LetterTextGrimmWandersIn".Translate();
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatSmall, pawn);
            return true;
        }
    }
}
