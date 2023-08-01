using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class GameStartView : BaseUIView
    {
        private const string START_BUTTON_NAME = "StartButton";

        public new class UxmlFactory : UxmlFactory<GameStartView> { }

        private Button m_startButton;

        public GameStartView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_startButton = this.Q<Button>(START_BUTTON_NAME);
            if (m_startButton != null)
            {
                m_startButton.clicked += OnStartButtonClick;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_startButton != null)
            {
                m_startButton.clicked -= OnStartButtonClick;
                m_startButton = null;
            }
        }

        private void OnStartButtonClick()
        {
            ProgressManager.Instance.ClearProgress();
            LevelManager.Instance.LoadLevel(0);
        }
    }
}
