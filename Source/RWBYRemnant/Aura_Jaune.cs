using UnityEngine;

namespace RWBYRemnant
{
    public class Aura_Jaune : Aura
    {
        public override Color GetColor()
        {
            return color;
        }

        public Color color = new Color(0.8f, 0.8f, 0.8f);
    }
}
