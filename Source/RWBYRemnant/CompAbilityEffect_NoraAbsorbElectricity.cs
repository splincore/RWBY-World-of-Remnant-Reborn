using RimWorld;
using Verse;

namespace RWBYRemnant
{
    public class CompAbilityEffect_NoraAbsorbElectricity : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.HasThing)
            {
                if (target.Thing.TryGetComp<CompPower>() is CompPower compPower && compPower.PowerNet.CurrentStoredEnergy() >= 200f)
                {
                    float energyToAbsorb = 200f;
                    for (int i = 0; i < compPower.PowerNet.batteryComps.Count; i++)
                    {
                        if (compPower.PowerNet.batteryComps[i].StoredEnergy >= energyToAbsorb)
                        {
                            compPower.PowerNet.batteryComps[i].DrawPower(energyToAbsorb);
                            break;
                        }
                        else
                        {
                            energyToAbsorb -= compPower.PowerNet.batteryComps[i].StoredEnergy;
                            compPower.PowerNet.batteryComps[i].DrawPower(compPower.PowerNet.batteryComps[i].StoredEnergy);
                        }
                    }
                    ChargePawn();
                    base.Apply(target, dest);
                }
                else if (target.Thing is Pawn pawn && pawn.RaceProps.IsMechanoid && !pawn.Dead)
                {
                    pawn.TakeDamage(new DamageInfo(DamageDefOf.EMP, 1f, instigator: parent.pawn));
                    ChargePawn();
                    base.Apply(target, dest);
                }
            }
        }

        public void ChargePawn()
        {
            Hediff hediffCharged = HediffMaker.MakeHediff(RWBYDefOf.RWBY_LightningBuff, parent.pawn);
            parent.pawn.health.AddHediff(hediffCharged);
            parent.pawn.TryGetComp<CompAura>().aura.CurrentEnergy = parent.pawn.TryGetComp<CompAura>().aura.maxEnergy;
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (base.Valid(target, throwMessages) && target.HasThing)
            {
                if (target.Thing.TryGetComp<CompPower>() is CompPower compPower && compPower.PowerNet.CurrentStoredEnergy() >= 200f)
                {
                    return true;
                }
                else if (target.Thing is Pawn pawn && pawn.RaceProps.IsMechanoid && !pawn.Dead)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
