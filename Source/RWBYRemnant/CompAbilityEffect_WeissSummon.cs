using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_WeissSummon : CompAbilityEffect
    {
        public new CompProperties_WeissSummonAbility Props => (CompProperties_WeissSummonAbility)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Thing timeDilationGlyph = ThingMaker.MakeThing(Props.thingToSpawn);
            timeDilationGlyph.TryGetComp<CompGlyphSummon>().SetData(GenTicks.SecondsToTicks(Props.timeToSummon), Props.summonToSpawn);
            GenSpawn.Spawn(timeDilationGlyph, target.Cell, parent.pawn.Map);
        }
    }
}
