using System;
using UnityEngine;

namespace DreamedReality.Inputs
{
    public interface IInputProvider
    {
        public event Action<Vector2> OnMoveActionUpdate;
        public event Action OnJumpActionDown;
        public event Action OnUseActionRelease;
        public event Action OnPauseActionRelease;
    }
}
