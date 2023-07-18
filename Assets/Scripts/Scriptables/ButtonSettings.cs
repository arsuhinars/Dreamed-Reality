using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "ButtonSettings", menuName = "Game/Tweens/Button Settings")]
    public class ButtonSettings : ScriptableObject
    {
        public Vector3 buttonMoveOffset;
        public float buttonMoveDuration;
    }
}
