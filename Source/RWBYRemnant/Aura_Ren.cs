﻿using UnityEngine;

namespace RWBYRemnant
{
    public class Aura_Ren : Aura
    {
        public override Color GetColor()
        {
            return color;
        }

        public Color color = new Color(1.0f, 0.6f, 1.0f);
    }
}
