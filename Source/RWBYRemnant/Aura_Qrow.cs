using Verse;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace RWBYRemnant
{
    public class Aura_Qrow : Aura
    {
        public override bool TryAbsorbDamage(DamageInfo dinfo)
        {
            if (CurrentEnergy > 0f && dinfo.Instigator is Pawn instigatorPawn && Rand.Chance(0.5f))
            {
                if (dinfo.Def.isRanged)
                {
                    switch (Rand.RangeInclusive(1, 4))
                    {
                        case 1:
                            RWBYDefOf.Ricochet1.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                            break;

                        case 2:
                            RWBYDefOf.Ricochet2.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                            break;

                        case 3:
                            RWBYDefOf.Ricochet3.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                            break;

                        case 4:
                            RWBYDefOf.Ricochet4.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                            break;
                    }
                    MoteMaker.ThrowText(instigatorPawn.DrawPos, instigatorPawn.Map, "TextMote_Crow_MissedRanged".Translate(), GetColor(), 3.65f);
                    return true;
                }
                else
                {
                    if (Rand.Chance(0.3f) && instigatorPawn.equipment.Primary != null)
                    {
                        instigatorPawn.equipment.TryDropEquipment(instigatorPawn.equipment.Primary, out ThingWithComps resultingEq, instigatorPawn.Position, false);
                    }
                    SoundDefOf.Pawn_Melee_Punch_Miss.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
                    MoteMaker.ThrowText(instigatorPawn.DrawPos, instigatorPawn.Map, "TextMote_Crow_MissedMelee".Translate(), GetColor(), 3.65f);
                    return true;
                }
            }
            return base.TryAbsorbDamage(dinfo);
        }

        public override Color GetColor()
        {
            return color;
        }

        public Color color = new Color(1.0f, 0f, 0f);
    }
}
