﻿using Verse;

namespace RWBYRemnant
{
    public class RemnantModSettings : ModSettings
    {
        public bool everyoneHasAura = false;
        public bool auraUnlockable = true;
        public bool semblanceUnlockable = true;
        public bool everyoneMakesPhotosForJoy = false;
        public bool hideAuraWhenMultiselect = false;
        public float earliestNuckelavee = 30f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref everyoneHasAura, "RemnantSettingAura", false, false);
            Scribe_Values.Look<bool>(ref auraUnlockable, "RemnantSettingAura", true, false);
            Scribe_Values.Look<bool>(ref semblanceUnlockable, "RemnantSettingAura", true, false);
            Scribe_Values.Look<bool>(ref everyoneMakesPhotosForJoy, "everyoneMakesPhotosForJoy", false, false);
            Scribe_Values.Look<bool>(ref hideAuraWhenMultiselect, "hideAuraWhenMultiselect", false, false);
            Scribe_Values.Look<float>(ref earliestNuckelavee, "earliestNuckelavee", 30f, false);
        }
    }
}
