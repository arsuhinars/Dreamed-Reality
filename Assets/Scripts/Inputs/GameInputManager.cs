using System;
using UnityEngine;

namespace DreamedReality.Inputs
{
    public class GameInputManager : MonoBehaviour
    {
        public event Action<Vector2> OnMoveActionUpdate;
        public event Action OnJumpActionDown;
        public event Action OnUseActionRelease;
        public event Action OnPauseActionRelease;

        public static GameInputManager Instance { get; private set; } = null;

        public void RegisterInputProvider(IInputProvider provider)
        {
            provider.OnMoveActionUpdate += HandleMoveActionUpdate;
            provider.OnJumpActionDown += HandleJumpActionRelease;
            provider.OnUseActionRelease += HandleUseActionRelease;
            provider.OnPauseActionRelease += HandlePauseActionRelease;
        }

        public void UnregisterInputProvider(IInputProvider provider)
        {
            provider.OnMoveActionUpdate -= HandleMoveActionUpdate;
            provider.OnJumpActionDown -= HandleJumpActionRelease;
            provider.OnUseActionRelease -= HandleUseActionRelease;
            provider.OnPauseActionRelease -= HandlePauseActionRelease;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Singleton already exists");
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void HandleMoveActionUpdate(Vector2 v)
        {
            OnMoveActionUpdate?.Invoke(v);
        }

        private void HandleJumpActionRelease()
        {
            OnJumpActionDown?.Invoke();
        }

        private void HandleUseActionRelease()
        {
            OnUseActionRelease?.Invoke();
        }

        private void HandlePauseActionRelease()
        {
            OnPauseActionRelease?.Invoke();
        }
    }
}
