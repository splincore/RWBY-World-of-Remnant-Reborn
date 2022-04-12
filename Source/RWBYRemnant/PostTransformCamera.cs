using Verse;

namespace RWBYRemnant
{
    public class PostTransformCamera : EquipmentToolbox.SpecialEffectsUtility
    {
        public override void DoPostTransformPreDestroyEvent(Pawn pawn, ThingWithComps transformableSource, ThingWithComps transformedInto, ThingWithComps secondaryProduct = null, Def neededEquippedItem = null)
        {
            if (transformableSource.TryGetComp<CompCameraPhotos>() != null && transformedInto.TryGetComp<CompCameraPhotos>() != null)
            {
                transformedInto.TryGetComp<CompCameraPhotos>().ListOfDifferentPhotos = transformableSource.TryGetComp<CompCameraPhotos>().ListOfDifferentPhotos;
            }
        }
    }
}
