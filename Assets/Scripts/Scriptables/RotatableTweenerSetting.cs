using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "RotatableTweenerSettings", menuName = "Game/Tweens/Rotatable Tweener Settings")]
    public class RotatableTweenerSetting : ScriptableObject
    {
        public Vector3 rotationAngle;
        public float rotationDuration;
    }
}
