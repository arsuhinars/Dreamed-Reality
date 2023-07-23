using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "CodeDisplaySettings", menuName = "Game/Entities/Code Display Settings")]
    public class CodeDisplaySettings : ScriptableObject
    {
        public int minCodeNumber;
        public int maxCodeNumber;
        public float numberChangeDuration;
        public SfxType usingSfx;
    }
}
