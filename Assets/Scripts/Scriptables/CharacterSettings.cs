using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Game/Entities/Character Settings")]
    public class CharacterSettings : ScriptableObject
    {
        public float maxMoveForce;
        public float flyMoveForce;
        public float jumpImpulse;
        public float rotationSmoothTime;
        [Space]
        public Vector3 groundDrag;
        public Vector3 airDrag;
        [Space]
        public float maxSlopeAngle;
        public LayerMask groundMask;
    }
}
