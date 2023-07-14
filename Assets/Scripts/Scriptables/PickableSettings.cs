using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "PickableSettings", menuName = "Game/Entities/Pickable Settings")]
    public class PickableSettings : ScriptableObject
    {
        public Vector3 moveAmplitude;
        public float movePeriod;
        public float rotationPeriod;
    }
}
