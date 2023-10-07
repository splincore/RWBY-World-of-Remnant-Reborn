using RimWorld;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class Grimm_DeathWorker : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            Color color = new Color(1, 1, 1); // White Smoke
            if (corpse.InnerPawn.Faction != Faction.OfPlayer)
            {
                color = new Color(0, 0, 0); // Black Smoke
                if (Rand.Chance(corpse.InnerPawn.RaceProps.AnyPawnKind.combatPower / 10000))
                {
                    ThingWithComps createdWeapon = (ThingWithComps)ThingMaker.MakeThing(RWBYDefOf.RWBY_Grimm_Glove);
                    createdWeapon.TryGetComp<CompQuality>().SetQuality((QualityCategory)Rand.RangeInclusive(0, 6), ArtGenerationContext.Colony);
                    GenSpawn.Spawn(createdWeapon, corpse.Position, corpse.Map);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(corpse.DrawPos, corpse.Map, FleckDefOf.Smoke, Rand.Range(2.5f, 4.5f));
                dataStatic.rotationRate = Rand.Range(-30f, 30f);
                dataStatic.velocityAngle = (float)Rand.Range(30, 40);
                dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
                dataStatic.instanceColor = color;
                corpse.Map.flecks.CreateFleck(dataStatic);
            }
            corpse.Destroy();
        }
    }
}
