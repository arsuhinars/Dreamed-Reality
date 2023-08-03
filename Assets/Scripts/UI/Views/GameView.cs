using DreamedReality.Inputs;
using DreamedReality.UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class GameView : BaseUIView
    {
        private const string MOBILE_INPUT_ELEMENT_NAME = "MobileInput";
        private const string MOVE_JOYSTICK_NAME = "MoveJoystick";
        private const string JUMP_BUTTON_NAME = "JumpButton";
        private const string USE_BUTTON_NAME = "UseButton";
        private const string PAUSE_BUTTON_NAME = "PauseButton";

        public new class UxmlFactory : UxmlFactory<GameView> { }

        private JoystickElement m_moveJoystick;
        private Button m_jumpBtn;
        private Button m_useBtn;
        private Button m_pauseBtn;

        public GameView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public MobileInputProvider CreateMobileInputProvider()
        {
            return new MobileInputProvider(m_moveJoystick, m_jumpBtn, m_useBtn, m_pauseBtn);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            var mobileInput = this.Q(MOBILE_INPUT_ELEMENT_NAME);
            mobileInput.style.display = (
                (Application.isMobilePlatform || Application.isEditor) ?
                DisplayStyle.Flex :
                DisplayStyle.None
            );

            m_moveJoystick = this.Q<JoystickElement>(MOVE_JOYSTICK_NAME);
            m_jumpBtn = this.Q<Button>(JUMP_BUTTON_NAME);
            m_useBtn = this.Q<Button>(USE_BUTTON_NAME);
            m_pauseBtn = this.Q<Button>(PAUSE_BUTTON_NAME);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            m_moveJoystick = null;
            m_jumpBtn = null;
            m_useBtn = null;
            m_pauseBtn = null;
        }
    }
}
