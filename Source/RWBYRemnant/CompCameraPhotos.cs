using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using Verse;

namespace RWBYRemnant
{
    public class CompCameraPhotos : CompUseEffect
    {
        public CompProperties_CameraPhotos Props => (CompProperties_CameraPhotos)props;

        public CompEquippable GetEquippable => parent.GetComp<CompEquippable>();

        public Pawn GetPawn => GetEquippable.verbTracker.PrimaryVerb.CasterPawn;

        public HashSet<string> ListOfDifferentPhotos = new HashSet<string>();

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref ListOfDifferentPhotos, false, parent.ThingID.ToString() + "ListOfDifferentPhotos");
            if (ListOfDifferentPhotos == null)
            {
                ListOfDifferentPhotos = new HashSet<string>();
            }
        }

        public void CreateLightCopy(string defname)
        {
            Pawn tmpPawn = GetPawn;
            if (GetPawn.equipment.Primary != null) //if primary equipped
            {
                if (GetPawn.equipment.Primary.TryGetComp<CompCameraPhotos>() == null) // if primary not camera
                {
                    if (GetPawn.equipment.Primary.TryGetComp<CompLightCopy>() != null) // if primary light copy
                    {
                        GetPawn.equipment.Primary.Destroy(); // destroy light copy
                    }
                    else
                    {
                        return; // primary is no camera or light copy
                    }
                }
            }

            // new weapon with specific color
            ThingWithComps weaponToCreate = new ThingWithComps();
            if (ThingDef.Named(defname).MadeFromStuff)
            {
                weaponToCreate = (ThingWithComps)ThingMaker.MakeThing(ThingDef.Named(defname), GenStuff.RandomStuffFor(ThingDef.Named(defname)));
            }
            else
            {
                weaponToCreate = (ThingWithComps)ThingMaker.MakeThing(ThingDef.Named(defname), null);
            }
            if (weaponToCreate.TryGetComp<CompQuality>() != null)
            {
                weaponToCreate.TryGetComp<CompQuality>().SetQuality(parent.TryGetComp<CompQuality>().Quality, ArtGenerationContext.Colony);
            }
            weaponToCreate.HitPoints = 1;
            if (weaponToCreate.TryGetComp<CompColorable>() == null)
            {
                CompColorable compColorable = new CompColorable
                {
                    parent = weaponToCreate
                };
                CompProperties compProps = new CompProperties
                {
                    compClass = typeof(CompColorable)
                };
                compColorable.Initialize(compProps);
                weaponToCreate.AllComps.Add(compColorable);
            }
            weaponToCreate.TryGetComp<CompColorable>().SetColor(Props.LightCopyColor);

            // remove certain special abilities from light copy
            weaponToCreate.AllComps.RemoveAll(x => x.GetType().Equals(typeof(EquipmentToolbox.CompTransformable)));
            weaponToCreate.AllComps.RemoveAll(x => x.GetType().Equals(typeof(CompCameraPhotos)));

            CompLightCopy rWBYLightCopyDestroyAbility = new CompLightCopy
            {
                parent = weaponToCreate
            };
            CompProperties compProps2 = new CompProperties
            {
                compClass = typeof(CompLightCopy)
            };
            rWBYLightCopyDestroyAbility.Initialize(compProps2);
            weaponToCreate.AllComps.Add(rWBYLightCopyDestroyAbility);

            ListOfDifferentPhotos.Remove(defname);
            tmpPawn.equipment.AddEquipment(weaponToCreate);
        }

        public void ClearPhotos()
        {
            ListOfDifferentPhotos.Clear();
        }        

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                action = ClearPhotos,
                defaultLabel = "LightCopyPhotoClearLabel".Translate(),
                defaultDesc = "LightCopyPhotoClearDescription".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
                disabled = ListOfDifferentPhotos.Count == 0,
                disabledReason = "LightCopyPhotoClearDisabled".Translate(),
                activateSound = SoundDefOf.CancelMode
            };

            foreach (string photoName in ListOfDifferentPhotos)
            {
                bool disabled = false;
                string disabledReason = "";

                if (GetPawn == null)
                {
                    disabled = true;
                    disabledReason = "DisabledNotEquipped".Translate(parent.def.label);
                }
                else if (!GetPawn.equipment.AllEquipmentListForReading.Contains(parent))
                {
                    disabled = true;
                    disabledReason = "DisabledNotEquipped".Translate(parent.def.label);
                }
                else if (parent.def != RWBYDefOf.RWBY_Anesidora_Box)
                {
                    disabled = true;
                    disabledReason = "DisabledNotInBoxForm".Translate().CapitalizeFirst();
                }

                yield return new Command_Action
                {
                    action = delegate ()
                    {
                        CreateLightCopy(photoName);
                    },
                    defaultLabel = "LightCopyLabel".Translate(ThingDef.Named(photoName).label),
                    defaultDesc = "LightCopyDescription".Translate(ThingDef.Named(photoName).label),
                    icon = ThingDef.Named(photoName).uiIcon,
                    defaultIconColor = Props.LightCopyColor,
                    disabled = disabled,
                    disabledReason = disabledReason,
                    alsoClickIfOtherInGroupClicked = false
                };
            }
        }
    }
}
