using System;
using UnityEngine;

namespace Assets.GameStateManagement
{
    public class PlayerSettings : ICloneable
    {
        public int MaxUnlockedLevel;

        public object Clone()
        {
            return new PlayerSettings
            {
                MaxUnlockedLevel = this.MaxUnlockedLevel
            };
        }
    }
}
