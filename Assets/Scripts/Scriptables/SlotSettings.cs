using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "SlotSettings", menuName = "Game/Entities/Slot Settings")]
    public class SlotSettings : ScriptableObject
    {
        public Vector3 putItemOffset;
        public float itemPuttingDuration;
    }
}
