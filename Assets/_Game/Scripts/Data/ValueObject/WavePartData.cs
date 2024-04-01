using System;
using Scripts.Enums;

namespace Scripts.Data.ValueObject
{
    [Serializable]
    public class WavePartData
    {
        public SoldierTypes soldierType;
        public int soldierCount;
    }
}