﻿using UnityEngine;

namespace RWBYRemnant
{
    public class Aura_Weiss : Aura
    {
        public override Color GetColor()
        {
            return color;
        }

        public Color color = new Color(0.6f, 0.8f, 1.0f);
    }
}
