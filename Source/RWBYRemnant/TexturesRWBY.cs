using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    [StaticConstructorOnStartup]
    public static class TexturesRWBY
    {
        public static readonly Texture2D UiDefaultClone = ContentFinder<Texture2D>.Get("Things/UI/UseDefaultClone", true);
        public static readonly Texture2D UiFireDust = ContentFinder<Texture2D>.Get("Things/UI/Fire_Dust", true);
        public static readonly Texture2D UiIceDust = ContentFinder<Texture2D>.Get("Things/UI/Ice_Dust", true);
        public static readonly Texture2D UiGravityDust = ContentFinder<Texture2D>.Get("Things/UI/Gravity_Dust_UI", true);
        public static readonly Texture2D UiAbility_YangReturn = ContentFinder<Texture2D>.Get("Things/UI/Ability_YangReturn", true);
    }
}
