using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "RotatableDoorSettings", menuName = "Game/Entities/Rotatable Door Settings")]
    public class RotatableDoorSettings : ScriptableObject
    {
        public float rotationAngle;
        public float rotationDuration;
    }
}
