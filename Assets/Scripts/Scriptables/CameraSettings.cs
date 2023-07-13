using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Game/Controllers/Camera Settings")]
    public class CameraSettings : ScriptableObject
    {
        public Vector3 lookDirection;
        public Vector3 lookCenterOffset;
        public float lookDistance;
        public float moveSmoothTime;
    }
}
