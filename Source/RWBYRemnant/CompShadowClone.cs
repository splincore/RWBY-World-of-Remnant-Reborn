using RimWorld;
using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class CompShadowClone : ThingComp
    {
        public CompProperties_ShadowClone Props => (CompProperties_ShadowClone)props;

        public void SetData(Pawn pawn, int tmpTicksToLive, Color color)
        {
            shadowClone = pawn;
            shadowCloneAngle = pawn.Rotation;
            if (color != new Color() && color != new Color(1.0f, 1.0f, 1.0f))
            {
                useColor = true;
                shadowCloneColor = color;
            }
            ticksToLive = tmpTicksToLive;
            GlobalTextureAtlasManager.TryGetPawnFrameSet(pawn, out pawnTextureAtlasFrameSet, out bool newFrameSet, true);
            material = MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout));
            material.color = color;
        }

        public void MovePawnOut()
        {
            if (parent.Position.GetFirstPawn(parent.Map) is Pawn pawn && pawn.Position == parent.Position)
            {
                bool pawnMoved = false;
                if (pawn.Rotation == Rot4.North)
                {
                    IntVec3 intVec = pawn.Position + new IntVec3(0, 0, -1);
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                    }
                }
                else if (pawn.Rotation == Rot4.East)
                {
                    IntVec3 intVec = pawn.Position + new IntVec3(-1, 0, 0);
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                    }
                }
                else if (pawn.Rotation == Rot4.South)
                {
                    IntVec3 intVec = pawn.Position + new IntVec3(0, 0, 1);
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                    }
                }
                else if (pawn.Rotation == Rot4.West)
                {
                    IntVec3 intVec = pawn.Position + new IntVec3(1, 0, 0);
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                    }
                }

                if (pawnMoved) return;

                for (int i = 1; i < 5; i++)
                {
                    IntVec3 intVec;

                    intVec = pawn.Position + new IntVec3(0, 0, -i); // move south
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        if (intVec == pawn.Position)
                        {
                            return;
                        }
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                        break;
                    }
                    intVec = pawn.Position + new IntVec3(-i, 0, 0); // move west
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        if (intVec == pawn.Position)
                        {
                            return;
                        }
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                        break;
                    }
                    intVec = pawn.Position + new IntVec3(0, 0, i); // move east
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        if (intVec == pawn.Position)
                        {
                            return;
                        }
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                        break;
                    }
                    intVec = pawn.Position + new IntVec3(i, 0, 0); // move north
                    if (PawnCanOccupy(intVec, pawn))
                    {
                        if (intVec == pawn.Position)
                        {
                            return;
                        }
                        pawn.Position = intVec;
                        pawn.Notify_Teleported(true, false);
                        pawnMoved = true;
                        break;
                    }
                }

                if (!pawnMoved) Log.Warning("Couldn´t move pawn " + pawn.Name.ToStringFull + " out of shadow clone.");
            }
        }

        private bool PawnCanOccupy(IntVec3 c, Pawn pawn)
        {
            if (!c.Walkable(pawn.Map))
            {
                return false;
            }
            Building edifice = c.GetEdifice(pawn.Map);
            if (edifice != null)
            {
                Building_Door building_Door = edifice as Building_Door;
                if (building_Door != null && !building_Door.PawnCanOpen(pawn) && !building_Door.Open)
                {
                    return false;
                }
            }
            return true;
        }

        public override void CompTick()
        {
            if (shadowClone == null)
            {
                parent.Destroy();
                return;
            }
            if (useColor && ticksToLive % 5 == 0)
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.DrawPos, shadowClone.Map, FleckDefOf.DustPuffThick, 2);
                dataStatic.rotationRate = (float)Rand.Range(-60, 60);
                dataStatic.velocityAngle = (float)Rand.Range(0, 360);
                dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
                dataStatic.instanceColor = shadowCloneColor;
                shadowClone.Map.flecks.CreateFleck(dataStatic);
            }
            ticksToLive--;
            if (ticksToLive < 1 || parent.Map != shadowClone.Map)
            {
                parent.Destroy();
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            if (parent.Map != shadowClone.Map) return;
            GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, shadowCloneAngle, PawnDrawMode.BodyAndHead), parent.DrawPos, Quaternion.AngleAxis(0f, Vector3.up), material, false);
        }

        private Mesh GetBlitMeshUpdatedFrame(PawnTextureAtlasFrameSet frameSet, Rot4 rotation, PawnDrawMode drawMode)
        {
            int index = frameSet.GetIndex(rotation, drawMode);
            if (frameSet.isDirty[index])
            {
                Find.PawnCacheCamera.rect = frameSet.uvRects[index];
                Find.PawnCacheRenderer.RenderPawn(shadowClone, frameSet.atlas, Vector3.zero, 1f, 0f, rotation, true, drawMode == PawnDrawMode.BodyAndHead, true, true, false, default(Vector3), null, null, false);
                Find.PawnCacheCamera.rect = new Rect(0f, 0f, 1f, 1f);
                frameSet.isDirty[index] = false;
            }
            return frameSet.meshes[index];
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref ticksToLive, "ticksAlive", 1, false);
            Scribe_Values.Look<bool>(ref useColor, "useColor", false, false);
            Scribe_Values.Look<Rot4>(ref shadowCloneAngle, "shadowCloneAngle", Rot4.South, false);
            Scribe_Values.Look<Color>(ref shadowCloneColor, "shadowCloneColor", new Color(), false);
            Scribe_Values.Look<PawnTextureAtlasFrameSet>(ref pawnTextureAtlasFrameSet, "pawnTextureAtlasFrameSet", null, false);
            Scribe_Values.Look<Material>(ref material, "material", null, false);
            Scribe_References.Look<Pawn>(ref shadowClone, "shadowClone", false);
        }

        public Pawn shadowClone;
        public Rot4 shadowCloneAngle;
        public Color shadowCloneColor;
        public int ticksToLive;
        public bool useColor;
        PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet;
        Material material;
    }
}
