using RimWorld;
using Verse;

namespace RWBYRemnant
{
    [DefOf]
    public static class RWBYDefOf
    {
        #region "general Defs"

        // Stats
        public static StatDef AuraCost;

        // Things
        public static ThingDef FireDustPowder;
        public static ThingDef IceDustPowder;
        public static ThingDef LightningDustPowder;
        public static ThingDef GravityDustPowder;
        public static ThingDef HardLightDustPowder;
        public static ThingDef FireDustCrystal;
        public static ThingDef IceDustCrystal;
        public static ThingDef LightningDustCrystal;
        public static ThingDef GravityDustCrystal;
        public static ThingDef HardLightDustCrystal;

        // History
        public static HistoryEventDef RWBY_AuraStolenByPlayer;

        // Thoughts
        public static ThoughtDef RWBY_AuraStolen_Relation;
        public static ThoughtDef RWBY_AuraStolen_Mood;

        // Traits
        public static TraitDef RWBY_Aura;

        // Apparel

        public static ThingDef Apparel_PumpkinPetes;
        public static ThingDef Apparel_PowderedDustBelt;
        public static ThingDef Apparel_DustCrystalBelt;

        // Hediffs
        public static HediffDef RWBY_ApathyTiredness;
        public static HediffDef RWBY_Thermos_Caffeine;
        public static HediffDef RWBY_InjectedFireCrystal;
        public static HediffDef RWBY_InjectedIceCrystal;
        public static HediffDef RWBY_InjectedLightningCrystal;
        public static HediffDef RWBY_InjectedGravityCrystal;
        public static HediffDef RWBY_InjectedHardLightCrystal;
        public static HediffDef RWBY_AuraStolen;
        public static HediffDef RWBY_SilverEyes;
        public static HediffDef RWBY_SilverEye_Exhaustion;

        // Jobs
        public static JobDef RWBY_StealAura;

        // Sounds
        public static SoundDef AuraFlicker;
        public static SoundDef AuraBreak;
        public static SoundDef Ricochet1;
        public static SoundDef Ricochet2;
        public static SoundDef Ricochet3;
        public static SoundDef Ricochet4;
        public static SoundDef Shoot_Fireball;
        public static SoundDef Draw_Gambol_Shroud_Katana;

        // Damage
        public static DamageDef RWBY_Inflame_Blunt;
        public static DamageDef Bomb_Fire;
        public static DamageDef RWBY_Ice_Blunt;
        public static DamageDef RWBY_Lightning_Blunt;
        public static DamageDef RWBY_Burn_Blunt;
        public static DamageDef Bomb_Lightning;
        public static DamageDef RWBY_Lightning_Slash;
        public static DamageDef RWBY_Lightning_Bullet;

        // Grimm
        public static FactionDef Creatures_of_Grimm;
        public static PawnKindDef Grimm_Boarbatusk;
        public static PawnKindDef Grimm_Beowolf;
        public static PawnKindDef Grimm_Ursa;
        public static PawnKindDef Grimm_Griffon;
        public static PawnKindDef Grimm_Nevermore;
        public static PawnKindDef Grimm_Lancer;
        public static PawnKindDef Grimm_LancerQueen;
        public static PawnKindDef Grimm_DeathStalker;
        public static PawnKindDef Grimm_Nuckelavee;
        public static PawnKindDef Grimm_Apathy;
        public static FleshTypeDef Grimm;
        public static ThingDef RWBY_Grimm_Glove;

        // Abilities General
        public static FleckDef RWBY_SilverEye_Fleck;
        public static SemblanceAbilityDef Ability_SilverEyes;

        #endregion

        #region "Character specific Defs"

        // Ruby
        public static TraitDef Semblance_Ruby;
        public static ThingDef RWBY_Crescent_Rose_Rifle;
        public static ThingDef RWBY_Crescent_Rose_Scythe;
        public static ThoughtDef RWBY_RubyUsedCrescentRose;
        public static SemblanceAbilityDef Ruby_BurstIntoRosePetals;
        public static SemblanceAbilityDef Ruby_CarryPawn;
        public static FleckDef RWBY_Rose_Petal;
        public static HediffDef RWBY_RubyDashForm;

        // Yang
        public static TraitDef Semblance_Yang;
        public static HediffDef RWBY_YangReturnDamage;

        // Weiss
        public static TraitDef Semblance_Weiss;
        public static ThingDef Weiss_Glyph_TimeDilation;
        public static HediffDef RWBY_TimeDilation;
        public static SemblanceAbilityDef Weiss_TimeDilationGlyph_Summon;
        public static SemblanceAbilityDef Weiss_SummonArmaGigas;
        public static SemblanceAbilityDef Weiss_SummonArmaGigasSword;
        public static SemblanceAbilityDef Weiss_SummonBoar;
        public static SemblanceAbilityDef Weiss_SummonBeowolf;
        public static SemblanceAbilityDef Weiss_SummonUrsa;
        public static SemblanceAbilityDef Weiss_SummonGriffon;
        public static SemblanceAbilityDef Weiss_SummonNevermore;
        public static SemblanceAbilityDef Weiss_SummonLancer;
        public static SemblanceAbilityDef Weiss_SummonLancerQueen;
        public static SemblanceAbilityDef Weiss_SummonDeathStalker;
        public static SemblanceAbilityDef Weiss_SummonNuckelavee;
        public static SemblanceAbilityDef Weiss_SummonApathy;

        // Blake
        public static TraitDef Semblance_Blake;
        public static ThingDef Blake_ShadowClone;
        public static ThingDef Blake_ShadowClone_Fire;
        public static ThingDef Blake_ShadowClone_Ice;
        public static ThingDef Blake_ShadowClone_Stone;

        // Nora
        public static TraitDef Semblance_Nora;
        public static HediffDef RWBY_LightningBuff;
        public static SemblanceAbilityDef Nora_AbsorbElectricity;

        // Jaune
        public static TraitDef Semblance_Jaune;
        public static ThingDef RWBY_PumpkinPetes;
        public static ThoughtDef RWBY_JauneAtePumpkinPetes;
        public static SemblanceAbilityDef Jaune_AmplifyAura;
        public static HediffDef RWBY_AmplifiedAura;

        // Pyrrha
        public static TraitDef Semblance_Pyrrha;
        public static SemblanceAbilityDef Pyrrha_UnlockAura;
        public static SemblanceAbilityDef Pyrrha_UseMagnetism;
        public static SemblanceAbilityDef Pyrrha_MagneticPulse;
        public static ThoughtDef RWBY_PyrrhaHurtFriendly;

        // Ren
        public static TraitDef Semblance_Ren;
        public static HediffDef RWBY_MaskedEmotions;
        public static SemblanceAbilityDef Ren_MaskEmotions;
        public static SemblanceAbilityDef Ren_MaskEmotionsAmplified;

        // Qrow
        public static TraitDef Semblance_Qrow;

        // Raven
        public static TraitDef Semblance_Raven;
        public static ThingDef Raven_Portal;
        public static JobDef GoThroughPortal;
        public static SemblanceAbilityDef Raven_FormBond;

        // Cinder
        public static TraitDef Semblance_Cinder;
        public static SemblanceAbilityDef Cinder_ShootFireCrystal;
        public static ThingDef Cinder_FireCrystal;
        public static SemblanceAbilityDef Cinder_SummonExplosives;
        public static ThingDef Cinder_ExplosiveMine;
        public static SemblanceAbilityDef Cinder_CreateScimitar;
        public static SemblanceAbilityDef Cinder_CreateSpear;
        public static SemblanceAbilityDef Cinder_CreateBlades;
        public static SemblanceAbilityDef Cinder_CreateBow;

        // Hazel
        public static TraitDef Semblance_Hazel;
        public static HediffDef RWBY_IgnorePain;

        // Velvet
        public static TraitDef Semblance_Velvet;
        public static ThingDef RWBY_Anesidora_Camera;
        public static ThingDef RWBY_Anesidora_Box;
        public static ThoughtDef RWBY_PictureTaken;
        public static JobDef RWBY_TakePhotos;
        public static HediffDef RWBY_VelvetMimicMoves;

        // Adam
        public static TraitDef Semblance_Adam;
        public static SemblanceAbilityDef Adam_UnleashDamage;

        #endregion
    }
}
