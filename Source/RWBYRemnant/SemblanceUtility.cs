using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RWBYRemnant
{
    public static class SemblanceUtility
    {
        public static readonly List<string> unlockSemblanceReasons = new List<string>
        {
            "BondedAnimalDied",
            "WitnessedDeathFamily",
            "GotMarried",
            "Catharsis",
        };

        public static readonly List<TraitDef> semblanceList = new List<TraitDef>
        {
            RWBYDefOf.Semblance_Ruby,
            RWBYDefOf.Semblance_Yang,
            RWBYDefOf.Semblance_Weiss,
            RWBYDefOf.Semblance_Blake,
            RWBYDefOf.Semblance_Nora,
            RWBYDefOf.Semblance_Jaune,
            RWBYDefOf.Semblance_Pyrrha,
            RWBYDefOf.Semblance_Ren,
            RWBYDefOf.Semblance_Qrow,
            RWBYDefOf.Semblance_Raven,
            RWBYDefOf.Semblance_Cinder,
            RWBYDefOf.Semblance_Hazel,
            RWBYDefOf.Semblance_Velvet,
            RWBYDefOf.Semblance_Adam,
        };

        public static readonly List<(HediffDef StatusType, DamageDef DamageType)> damageBoostingHediffs = new List<(HediffDef, DamageDef)>
        {
            (RWBYDefOf.RWBY_YangReturnDamage, DamageDefOf.Blunt),
            (RWBYDefOf.RWBY_LightningBuff, RWBYDefOf.RWBY_Lightning_Blunt),
            (RWBYDefOf.RWBY_InjectedFireCrystal, RWBYDefOf.RWBY_Inflame_Blunt),
            (RWBYDefOf.RWBY_InjectedIceCrystal, RWBYDefOf.RWBY_Ice_Blunt),
            (RWBYDefOf.RWBY_InjectedLightningCrystal, RWBYDefOf.RWBY_Lightning_Blunt),
            (RWBYDefOf.RWBY_InjectedGravityCrystal, DamageDefOf.Blunt),
            (RWBYDefOf.RWBY_InjectedHardLightCrystal, RWBYDefOf.RWBY_Burn_Blunt),
        };

        public static readonly List<string> noraDmgAbsorbDefs = new List<string>
        {
            "TM_Lightning",
            "TM_LightningCloud",
            "SW_ShockBullet_Damage",
            "SW_ShockBullet_Damage_Penetrating",
            "Rimlaser_TeslaGun",
            "PJ_FLightning",
        };

        public static readonly List<HediffDef> injectedDustCrystalHediffs = new List<HediffDef>
        {
            RWBYDefOf.RWBY_InjectedFireCrystal,
            RWBYDefOf.RWBY_InjectedIceCrystal,
            RWBYDefOf.RWBY_InjectedLightningCrystal,
            RWBYDefOf.RWBY_InjectedGravityCrystal,
            RWBYDefOf.RWBY_InjectedHardLightCrystal,
        };

        public static IEnumerable<TraitDef> GetSemblancesForPassion(SkillDef skill)
        {
            if (skill == SkillDefOf.Shooting)
            {
                yield return RWBYDefOf.Semblance_Ruby;
                yield return RWBYDefOf.Semblance_Blake;
                yield return RWBYDefOf.Semblance_Cinder;
            }
            if (skill == SkillDefOf.Melee)
            {
                yield return RWBYDefOf.Semblance_Yang;
                yield return RWBYDefOf.Semblance_Hazel;
                yield return RWBYDefOf.Semblance_Qrow;
                yield return RWBYDefOf.Semblance_Adam;
            }
            if (skill == SkillDefOf.Construction)
            {
                yield return RWBYDefOf.Semblance_Raven;
                yield return RWBYDefOf.Semblance_Velvet;
                yield return RWBYDefOf.Semblance_Pyrrha;
            }
            if (skill == SkillDefOf.Mining)
            {
                yield return RWBYDefOf.Semblance_Hazel;
                yield return RWBYDefOf.Semblance_Qrow;
                yield return RWBYDefOf.Semblance_Jaune;
            }
            if (skill == SkillDefOf.Cooking)
            {
                yield return RWBYDefOf.Semblance_Ren;
                yield return RWBYDefOf.Semblance_Qrow;
                yield return RWBYDefOf.Semblance_Nora;
            }
            if (skill == SkillDefOf.Plants)
            {
                yield return RWBYDefOf.Semblance_Yang;
                yield return RWBYDefOf.Semblance_Velvet;
                yield return RWBYDefOf.Semblance_Raven;
                yield return RWBYDefOf.Semblance_Jaune;
            }
            if (skill == SkillDefOf.Animals)
            {
                yield return RWBYDefOf.Semblance_Weiss;
                yield return RWBYDefOf.Semblance_Blake;
                yield return RWBYDefOf.Semblance_Yang;
                yield return RWBYDefOf.Semblance_Adam;
            }
            if (skill == SkillDefOf.Crafting)
            {
                yield return RWBYDefOf.Semblance_Ren;
                yield return RWBYDefOf.Semblance_Pyrrha;
                yield return RWBYDefOf.Semblance_Cinder;
                yield return RWBYDefOf.Semblance_Adam;
            }
            if (skill == SkillDefOf.Artistic)
            {
                yield return RWBYDefOf.Semblance_Ruby;
                yield return RWBYDefOf.Semblance_Nora;
                yield return RWBYDefOf.Semblance_Blake;
            }
            if (skill == SkillDefOf.Medicine)
            {
                yield return RWBYDefOf.Semblance_Raven;
                yield return RWBYDefOf.Semblance_Jaune;
                yield return RWBYDefOf.Semblance_Pyrrha;
                yield return RWBYDefOf.Semblance_Hazel;
            }
            if (skill == SkillDefOf.Social)
            {
                yield return RWBYDefOf.Semblance_Ruby;
                yield return RWBYDefOf.Semblance_Weiss;
                yield return RWBYDefOf.Semblance_Velvet;
                yield return RWBYDefOf.Semblance_Nora;
            }
            if (skill == SkillDefOf.Intellectual)
            {
                yield return RWBYDefOf.Semblance_Weiss;
                yield return RWBYDefOf.Semblance_Ren;
                yield return RWBYDefOf.Semblance_Cinder;
            }
            yield break;
        }

        public static bool TryUnlockSemblance(Pawn pawn, TraitDef semblance, string textKey, bool isInitialization = false, string labelKey = "LetterLabelUnlockSemblanceGeneral")
        {
            if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen)) return false;
            if (!isInitialization && !pawn.story.traits.HasTrait(RWBYDefOf.RWBY_Aura)) return false;
            if (!isInitialization && pawn.WorkTagIsDisabled(semblance.requiredWorkTags)) return false;
            pawn.story.traits.allTraits.RemoveAll(t => t.def.Equals(RWBYDefOf.RWBY_Aura));
            if (!pawn.story.traits.HasTrait(semblance)) pawn.story.traits.GainTrait(new Trait(semblance));

            if (semblance == RWBYDefOf.Semblance_Ruby)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Ruby { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
                pawn.abilities.GainAbility(RWBYDefOf.Ruby_BurstIntoRosePetals);
                pawn.abilities.GainAbility(RWBYDefOf.Ruby_CarryPawn);
                // TODO add abilities
            }
            else if (semblance == RWBYDefOf.Semblance_Yang)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Yang { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Weiss)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Weiss { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Blake)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Blake { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Nora)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Nora { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Jaune)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Jaune { maxEnergy = 150, CurrentEnergy = 150, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Pyrrha)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Pyrrha { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Ren)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Ren { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Qrow)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Qrow { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Raven)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Raven { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Cinder)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Cinder { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Hazel)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Hazel { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Velvet)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Velvet { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }
            else if (semblance == RWBYDefOf.Semblance_Adam)
            {
                pawn.TryGetComp<CompAura>().aura = new Aura_Adam { maxEnergy = 100, CurrentEnergy = 100, pawn = pawn };
            }

            string label = labelKey.Translate().Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN").CapitalizeFirst();
            string text = textKey.Translate().Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN").CapitalizeFirst();
            if (pawn.IsColonistPlayerControlled && pawn.Spawned) Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn);
            return true;
        }

        public static bool TryUnlockAura(Pawn pawn, string textKey, bool isInitialization = false, string labelKey = "LetterLabelUnlockAura", float auraSize = 70f)
        {
            if (pawn.health.hediffSet.HasHediff(RWBYDefOf.RWBY_AuraStolen)) return false;
            if (!LoadedModManager.GetMod<RemnantMod>().GetSettings<RemnantModSettings>().auraUnlockable) return false;
            if (!isInitialization && pawn.story.traits.HasTrait(RWBYDefOf.RWBY_Aura)) return false;
            if (semblanceList.Any(s => pawn.story.traits.HasTrait(s))) return false;
            if (!isInitialization)
            {
                pawn.story.traits.GainTrait(new Trait(RWBYDefOf.RWBY_Aura));
            }
            pawn.TryGetComp<CompAura>().aura = new Aura { maxEnergy = auraSize, CurrentEnergy = auraSize, pawn = pawn };

            string label = labelKey.Translate().Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN").CapitalizeFirst();
            string text = textKey.Translate().Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN").CapitalizeFirst();
            if (pawn.IsColonistPlayerControlled && pawn.Spawned) Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn);
            return true;
        }

        //public static readonly List<PawnKindDef> summonedPawnKindDefs = new List<PawnKindDef>
        //{
        //    RWBYDefOf.Grimm_Boarbatusk_Summoned,
        //    RWBYDefOf.Grimm_ArmaGigas_Summoned,
        //    RWBYDefOf.Grimm_ArmaGigasSword_Summoned
        //};

        public static bool PyrrhaMagnetismCanAffect(Thing thing)
        {
            if (thing == null) return false;
            if (thing is ThingWithComps thingWithComps && (thingWithComps.Smeltable || thingWithComps.def.IsMetal)) return true;
            if (thing is Pawn pawn && (pawn.RaceProps.IsMechanoid || pawn.RaceProps.FleshType.defName == "ChJDroid" || pawn.RaceProps.FleshType.defName == "Android" || pawn.RaceProps.FleshType.defName == "MechanisedInfantry")) return true;

            return false;
        }

        public static bool PyrrhaMagnetismCanAffect(ThingDef thingDef)
        {
            if (thingDef == null) return false;
            if (thingDef.smeltable || thingDef.IsMetal) return true;
            if (thingDef.race is RaceProperties raceProperties && (raceProperties.IsMechanoid || raceProperties.FleshType.defName == "ChJDroid" || raceProperties.FleshType.defName == "Android" || raceProperties.FleshType.defName == "MechanisedInfantry")) return true;

            return false;
        }
    }
}
