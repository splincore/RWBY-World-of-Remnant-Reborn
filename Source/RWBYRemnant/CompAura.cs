using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RWBYRemnant
{
    public class CompAura : ThingComp
    {
        public CompProperties_Aura Props => (CompProperties_Aura)props;

        public Pawn AuraOwner => (Pawn)parent;

        public void Initialize()
        {
            if (hiddenSemblance == null) GenerateHiddenSemblance();
            initialized = true;
            if (AuraOwner.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen)) return;
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.RWBY_Aura)) SemblanceUtility.UnlockAura(AuraOwner, "LetterTextUnlockAuraAuto");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Ruby)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Ruby, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Yang)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Yang, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Weiss)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Weiss, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Blake)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Blake, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Nora)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Nora, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Jaune)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Jaune, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Pyrrha)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Pyrrha, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Ren)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Ren, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Qrow)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Qrow, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Raven)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Raven, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Cinder)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Cinder, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Hazel)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Hazel, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Velvet)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Velvet, "LetterTextUnlockSemblanceGeneral");
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Adam)) SemblanceUtility.UnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Adam, "LetterTextUnlockSemblanceGeneral");
            if (LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().everyoneHasAura) SemblanceUtility.UnlockAura(AuraOwner, "LetterTextUnlockAuraAuto", auraSize: 30f);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (!initialized)
            {
                Initialize();
            }
            if (aura != null)
            {
                aura.Tick();
            }
            else if(!AuraOwner.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen))
            {
                if (auraAutoUnlock > 0)
                {
                    auraAutoUnlock--;
                }
                else
                {
                    SemblanceUtility.UnlockAura(AuraOwner, "LetterTextUnlockAuraAuto"); // TODO unlock Aura
                }
            }
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            if (AuraOwner.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen))
            {
                aura = null;
                return;
            }
            if (aura != null)
            {
                aura.TickRare();
                if (!AuraOwner.story.traits.allTraits.Any(t => t.def == RWBYDefOf.RWBY_Aura || SemblanceUtility.semblanceList.Contains(t.def)) && !LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().everyoneHasAura)
                {
                    aura = null;
                }
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            aura?.Draw();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Find.Selector.NumSelected > 1 && LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().hideAuraWhenMultiselect) yield break;
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            if (aura != null)
            {
                foreach (Gizmo gizmo in aura.GetGizmos())
                {
                    yield return gizmo;
                }
            }
        }

        public bool TryUnlockSemblanceWith(SkillDef skillDef, bool forceUnlock = false)
        {
            if (forceUnlock)
            {
                return SemblanceUtility.UnlockSemblance(AuraOwner, hiddenSemblance, "LetterTextUnlockSemblanceGeneral");
            }
            if (SemblanceUtility.GetSemblancesForPassion(skillDef).Contains(hiddenSemblance))
            {
                return SemblanceUtility.UnlockSemblance(AuraOwner, hiddenSemblance, "LetterTextUnlock" + hiddenSemblance.defName.Replace("_", ""));
            }
            return false;
        }

        public void Notify_EatenPumkinPetes()
        {
            eatenPumkinPetesCounter += 1;
            if (eatenPumkinPetesCounter == 50)
            {
                eatenPumkinPetesCounter = 0;
                List<Thing> things = new List<Thing>();
                ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(RWBYDefOf.Apparel_PumpkinPetes, GenStuff.RandomStuffFor(RWBYDefOf.Apparel_PumpkinPetes));
                thingWithComps.TryGetComp<CompQuality>().SetQuality((QualityCategory)Rand.RangeInclusive(0, 6), ArtGenerationContext.Colony);
                things.Add(thingWithComps);
                IntVec3 intVec = DropCellFinder.RandomDropSpot(AuraOwner.Map);
                DropPodUtility.DropThingsNear(intVec, AuraOwner.Map, things, 110, false, false, false);
                Find.LetterStack.ReceiveLetter("LetterLabelPumpkinPetePodCrash".Translate(), "LetterTextPumpkinPetePodCrash".Translate(), LetterDefOf.PositiveEvent, new TargetInfo(intVec, AuraOwner.Map, false), null, null);
            }
        }

        public void GenerateHiddenSemblance()
        {
            if (AuraOwner.relations != null && AuraOwner.relations.RelatedPawns.Any(p => p.relations.Children.Contains(AuraOwner) && p.story.traits.HasTrait(RWBYDefOf.Semblance_Weiss)) || AuraOwner.relations.Children.Any(c => c.story.traits.HasTrait(RWBYDefOf.Semblance_Weiss)))
            {
                hiddenSemblance = RWBYDefOf.Semblance_Weiss;
                return;
            }
            List<TraitDef> traitDefs = new List<TraitDef>();
            foreach (SkillRecord skillRecord in AuraOwner.skills.skills)
            {
                if (skillRecord.passion == Passion.Minor)
                {
                    traitDefs.AddRange(SemblanceUtility.GetSemblancesForPassion(skillRecord.def));
                }
                else if (skillRecord.passion == Passion.Major)
                {
                    traitDefs.AddRange(SemblanceUtility.GetSemblancesForPassion(skillRecord.def));
                    traitDefs.AddRange(SemblanceUtility.GetSemblancesForPassion(skillRecord.def));
                }
            }
            traitDefs.RemoveAll(t => AuraOwner.WorkTagIsDisabled(t.requiredWorkTags));
            if (traitDefs.Count == 0)
            {
                hiddenSemblance = SemblanceUtility.semblanceList.FindAll(s => !AuraOwner.WorkTagIsDisabled(s.requiredWorkTags)).RandomElement(); // should never be empty, as there are Semblances without required workTags
            }
            else
            {
                List<TraitDef> allPossibleTraits = traitDefs.Distinct().ToList();
                Dictionary<TraitDef, int> keyValuePairs = new Dictionary<TraitDef, int>();
                foreach (TraitDef traitDef in traitDefs)
                {
                    if (keyValuePairs.ContainsKey(traitDef))
                    {
                        keyValuePairs[traitDef]++;
                    }
                    else
                    {
                        keyValuePairs.Add(traitDef, 1);
                    }
                }
                int highestCount = keyValuePairs.Values.ToList().OrderByDescending(i => i).First();
                List<TraitDef> mostMatchingTraits = new List<TraitDef>();
                foreach (KeyValuePair<TraitDef, int> keyValuePair in keyValuePairs)
                {
                    if (keyValuePair.Value == highestCount) mostMatchingTraits.Add(keyValuePair.Key);
                }
                hiddenSemblance = mostMatchingTraits.RandomElement();
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref initialized, "initialized", false, false);
            Scribe_Deep.Look(ref aura, false, parent.ThingID.ToString() + "Aura");
            Scribe_Values.Look<int>(ref eatenPumkinPetesCounter, "eatenPumkinPetesCounter", 0, false);
            Scribe_Defs.Look<TraitDef>(ref hiddenSemblance, "hiddenSemblance");
            Scribe_Values.Look<int>(ref auraAutoUnlock, "auraAutoUnlock", Rand.Range(3600000, 7200000), false);
        }

        public bool initialized;
        public Aura aura = null;
        public int eatenPumkinPetesCounter = 0;
        public TraitDef hiddenSemblance = null;
        public int auraAutoUnlock = Rand.RangeInclusive(3600000, 7200000); // between 1 and 2 ingame years
    }
}
