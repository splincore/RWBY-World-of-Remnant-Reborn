using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_WeissTimeDilation : CompAbilityEffect
    {
        public new CompProperties_WeissTimeDilation Props => (CompProperties_WeissTimeDilation)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Thing timeDilationGlyph = ThingMaker.MakeThing(Props.thingToSpawn);
            timeDilationGlyph.TryGetComp<CompTimeDilationGlyph>().SetData(GenTicks.SecondsToTicks(10f));
            GenSpawn.Spawn(timeDilationGlyph, target.Cell, parent.pawn.Map);
        }
    }
}
