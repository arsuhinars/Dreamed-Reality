using System;
using UnityEngine;

namespace DreamedReality.Scriptables
{
    [Serializable]
    public struct LevelData
    {
        public string sceneName;
    }

    [CreateAssetMenu(fileName = "LevelsSettings", menuName = "Game/Managers/Levels Settings")]
    public class LevelsSettings : ScriptableObject
    {
        public float minLoadingTime;
        public string mainMenuSceneName;
        [Space]
        public LevelData[] levels;
    }
}
