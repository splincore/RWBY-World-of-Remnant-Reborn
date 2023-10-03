using RimWorld;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    [StaticConstructorOnStartup]
    public class Command_AbilitySemblance : Command_Ability
    {
        public Command_AbilitySemblance(Ability ability) : base(ability)
        {
        }

        public override Texture2D BGTexture
        {
            get
            {
                return TexturesRWBY.BGTexDefaultRWBY;
            }
        }

        public override Texture2D BGTextureShrunk
        {
            get
            {
                return TexturesRWBY.BGTexShrunkDefaultRWBY;
            }
        }
    }
}
