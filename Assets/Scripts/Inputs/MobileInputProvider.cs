using DreamedReality.UI.Elements;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamedReality.Inputs
{
    public class MobileInputProvider : IInputProvider
    {
        public event Action<Vector2> OnMoveActionUpdate;
        public event Action OnJumpActionDown;
        public event Action OnUseActionRelease;
        public event Action OnPauseActionRelease;

        private JoystickElement m_moveJoystick;
        private Button m_jumpButton;
        private Button m_useButton;
        private Button m_pauseButton;

        public MobileInputProvider(
            JoystickElement moveJoystick,
            Button jumpButton,
            Button useButton,
            Button pauseButton
        ) {
            m_moveJoystick = moveJoystick;
            m_jumpButton = jumpButton;
            m_useButton = useButton;
            m_pauseButton = pauseButton;

            m_moveJoystick.OnUpdate += OnMoveJoystickUpdate;
            m_jumpButton.RegisterCallback<PointerDownEvent>(OnJumpButtonDown, TrickleDown.TrickleDown);
            m_useButton.RegisterCallback<PointerUpEvent>(OnUseButtonUp);
            m_pauseButton.RegisterCallback<PointerUpEvent>(OnPauseButtonUp);
        }

        ~MobileInputProvider()
        {
            m_moveJoystick.OnUpdate -= OnMoveJoystickUpdate;
            m_jumpButton.UnregisterCallback<PointerDownEvent>(OnJumpButtonDown);
            m_useButton.UnregisterCallback<PointerUpEvent>(OnUseButtonUp);
            m_pauseButton.UnregisterCallback<PointerUpEvent>(OnPauseButtonUp);
        }

        private void OnMoveJoystickUpdate(Vector2 v)
        {
            OnMoveActionUpdate?.Invoke(v);
        }

        private void OnJumpButtonDown(PointerDownEvent ev)
        {
            OnJumpActionDown?.Invoke();
        }

        private void OnUseButtonUp(PointerUpEvent ev)
        {
            OnUseActionRelease?.Invoke();
        }

        private void OnPauseButtonUp(PointerUpEvent ev)
        {
            OnPauseActionRelease?.Invoke();
        }
    }
}
