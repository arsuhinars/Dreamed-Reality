using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "SlideDoorSettings", menuName = "Game/Tweens/Slide Door Settings")]
    public class SlideDoorSettings : ScriptableObject
    {
        public Vector3 slideOffset;
        public float slideDuration;
    }
}
