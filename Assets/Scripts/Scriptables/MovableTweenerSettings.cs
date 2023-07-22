using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "MovableTweenerSettings", menuName = "Game/Tweens/Movable Tweener Settings")]
    public class MovableTweenerSettings : ScriptableObject
    {
        public Vector3 slideOffset;
        public float slideDuration;
    }
}
