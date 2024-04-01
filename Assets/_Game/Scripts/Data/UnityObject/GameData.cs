using Scripts.Data.ValueObject;
using UnityEngine;

namespace Scripts.Data.UnityObject
{
    [CreateAssetMenu(fileName = "GameData", menuName = "3dDefence/GameData",order=0)]
    public class GameData : ScriptableObject
    {
        public LevelData[] LevelData;
    }
}