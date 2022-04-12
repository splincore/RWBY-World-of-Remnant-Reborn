using UnityEngine;
using Verse;

namespace RWBYRemnant
{
    public class CompProperties_CameraPhotos : CompProperties
    {
        public Color LightCopyColor;

        public CompProperties_CameraPhotos()
        {
            compClass = typeof(CompCameraPhotos);
        }
    }
}
