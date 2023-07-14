using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class GameView : BaseUIView
    {
        private const string PAUSE_BUTTON_NAME = "PauseButton";

        public new class UxmlFactory : UxmlFactory<GameView> { }

        private Button m_pauseBtn;

        public GameView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_pauseBtn = this.Q<Button>(PAUSE_BUTTON_NAME);
            if (m_pauseBtn != null)
            {
                m_pauseBtn.clicked += OnPauseButtonClicked;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_pauseBtn != null)
            {
                m_pauseBtn.clicked -= OnPauseButtonClicked;
                m_pauseBtn = null;
            }
        }

        private void OnPauseButtonClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PauseGame();
            }
        }
    }
}
