using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RWBYRemnant
{
    public class Projectile_RubyCarry : Bullet
    {
        public ThingDef_RubyCarry Def
        {
            get
            {
                return def as ThingDef_RubyCarry;
            }
        }

        public override void Tick()
        {
            if (launcher is Pawn launcherPawn && launcherPawn != null)
            {
                if (launcherPawn.Downed || launcherPawn.Dead)
                {
                    launcherPawn.health.hediffSet.hediffs.RemoveAll(h => h.def == RWBYDefOf.RWBY_RubyDashForm);
                    this.Destroy();
                    return;
                }
                Find.Selector.Deselect(launcher);
                Hediff dashHediff = new Hediff();
                dashHediff = HediffMaker.MakeHediff(RWBYDefOf.RWBY_RubyDashForm, launcherPawn);
                launcherPawn.health.AddHediff(dashHediff);
                launcherPawn.Position = Position;
                launcherPawn.Notify_Teleported(true, false);
            }
            if (carriedPawn != null)
            {
                carriedPawn.Position = Position;
                carriedPawn.Notify_Teleported(true, false);
            }
            FleckMaker.ThrowDustPuffThick(Position.ToVector3(), Map, 2, Def.color);
            if (Find.TickManager.TicksGame % 2 == 0)
            {
                if (!Position.ShouldSpawnMotesAt(Map))
                {
                    return;
                }
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(Position.ToVector3(), Map, RWBYDefOf.RWBY_Rose_Petal, 1.9f);
                dataStatic.rotationRate = 0f;
                dataStatic.velocityAngle = (float)Rand.Range(0, 360);
                dataStatic.velocitySpeed = 0.2f;
                Map.flecks.CreateFleck(dataStatic);
            }
            base.Tick();
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (launcher is Pawn launcherPawn && launcherPawn != null)
            {
                launcherPawn.Position = Position;
                launcherPawn.Notify_Teleported(true, false);
                launcherPawn.health.hediffSet.hediffs.RemoveAll(h => h.def == RWBYDefOf.RWBY_RubyDashForm);
                Find.Selector.Select(launcher);
            }
            if (carriedPawn != null)
            {
                carriedPawn.Position = Position;
                carriedPawn.Notify_Teleported(true, false);
            }
            base.Impact(hitThing, blockedByShield);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref carriedPawn, "carriedPawn", false);
        }

        public Pawn carriedPawn;
    }
}
