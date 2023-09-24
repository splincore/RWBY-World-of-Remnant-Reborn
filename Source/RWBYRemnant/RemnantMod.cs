using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class RemnantMod : Mod
    {
        readonly RemnantModSettings remnantModSettings;

        public RemnantMod(ModContentPack content) : base(content)
        {
            this.remnantModSettings = GetSettings<RemnantModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("AuraEveryoneLabel".Translate(), ref remnantModSettings.everyoneHasAura, "AuraEveryoneTooltip".Translate());
            listingStandard.CheckboxLabeled("AuraLabel".Translate(), ref remnantModSettings.auraUnlockable, "AuraTooltip".Translate());
            listingStandard.CheckboxLabeled("SemblanceLabel".Translate(), ref remnantModSettings.semblanceUnlockable, "SemblanceTooltip".Translate());
            listingStandard.CheckboxLabeled("EveryoneMakesPhotosForJoyLabel".Translate(), ref remnantModSettings.everyoneMakesPhotosForJoy, "EveryoneMakesPhotosForJoyTooltip".Translate());
            listingStandard.CheckboxLabeled("HideAuraWhenMultiselectLabel".Translate(), ref remnantModSettings.hideAuraWhenMultiselect, "HideAuraWhenMultiselectTooltip".Translate());

            listingStandard.Label("EarliestNuckelaveeLabel".Translate());
            listingStandard.Gap(listingStandard.verticalSpacing);
            Rect rect1 = listingStandard.GetRect(22f);
            Widgets.HorizontalSlider(rect1, ref remnantModSettings.earliestNuckelavee, rangeEarliestNuckelavee, remnantModSettings.earliestNuckelavee.ToString(""), 1f);
            //remnantModSettings.earliestNuckelavee = Widgets.HorizontalSlider(rect1, ref remnantModSettings.earliestNuckelavee, rangeEarliestNuckelavee, remnantModSettings.earliestNuckelavee.ToString(""), 1f);

            listingStandard.End();
        }

        public override string SettingsCategory()
        {
            return "RWBY World of Remnant";
        }

        private readonly FloatRange rangeEarliestNuckelavee = new FloatRange(0f, 120f);
    }
}
