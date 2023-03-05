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
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.RWBY_Aura)) SemblanceUtility.TryUnlockAura(AuraOwner, "LetterTextUnlockAuraAuto", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Ruby)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Ruby, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Yang)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Yang, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Weiss)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Weiss, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Blake)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Blake, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Nora)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Nora, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Jaune)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Jaune, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Pyrrha)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Pyrrha, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Ren)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Ren, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Qrow)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Qrow, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Raven)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Raven, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Cinder)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Cinder, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Hazel)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Hazel, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Velvet)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Velvet, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (AuraOwner.story.traits.HasTrait(RWBYDefOf.Semblance_Adam)) SemblanceUtility.TryUnlockSemblance(AuraOwner, RWBYDefOf.Semblance_Adam, "LetterTextUnlockSemblanceGeneral", isInitialization: true);
            if (LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().everyoneHasAura) SemblanceUtility.TryUnlockAura(AuraOwner, "LetterTextUnlockAuraAuto", isInitialization: true, auraSize: 30f);
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
                if (aura.maxEnergy < 50f)
                {
                    ProgressAutoUnlock();
                }
            }
            else if(!AuraOwner.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen))
            {
                ProgressAutoUnlock();
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
                return SemblanceUtility.TryUnlockSemblance(AuraOwner, hiddenSemblance, "LetterTextUnlockSemblanceGeneral");
            }
            if (SemblanceUtility.GetSemblancesForPassion(skillDef).Contains(hiddenSemblance))
            {
                return SemblanceUtility.TryUnlockSemblance(AuraOwner, hiddenSemblance, "LetterTextUnlock" + hiddenSemblance.defName.Replace("_", ""));
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

        public void ProgressAutoUnlock()
        {
            if (auraAutoUnlock > 0)
            {
                auraAutoUnlock--;
            }
            else
            {
                SemblanceUtility.TryUnlockAura(AuraOwner, "LetterTextUnlockAuraAuto"); // TODO unlock Aura
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref initialized, "initialized", false, false);
            Scribe_Deep.Look<Aura>(ref aura, false, parent.ThingID.ToString() + "Aura");
            Scribe_Values.Look<int>(ref eatenPumkinPetesCounter, "eatenPumkinPetesCounter", 0, false);
            Scribe_Defs.Look<TraitDef>(ref hiddenSemblance, "hiddenSemblance");
            Scribe_Values.Look<int>(ref auraAutoUnlock, "auraAutoUnlock", Rand.Range(3600000, 7200000), false);
        }

        public bool initialized = false;
        public Aura aura = null;
        public int eatenPumkinPetesCounter = 0;
        public TraitDef hiddenSemblance = null;
        public int auraAutoUnlock = Rand.RangeInclusive(3600000, 7200000); // between 1 and 2 ingame years
    }
}
