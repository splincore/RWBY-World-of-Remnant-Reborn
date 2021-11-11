﻿using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class RemnantMod : Mod
    {
        RemnantModSettings remnantModSettings;

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
            listingStandard.CheckboxLabeled("FixTraitDisabledWorkTagsLabel".Translate(), ref remnantModSettings.fixTraitDisabledWorkTags, "FixTraitDisabledWorkTagsTooltip".Translate());

            listingStandard.Label("EarliestNuckelaveeLabel".Translate());
            listingStandard.Gap(listingStandard.verticalSpacing);
            Rect rect1 = listingStandard.GetRect(22f);
            remnantModSettings.earliestNuckelavee = Widgets.HorizontalSlider(rect1, remnantModSettings.earliestNuckelavee, 0f, 120f, false, (remnantModSettings.earliestNuckelavee).ToString("") + "", "0", "120", 1f);

            listingStandard.End();
        }

        public override string SettingsCategory()
        {
            return "RWBY World of Remnant";
        }
    }
}
