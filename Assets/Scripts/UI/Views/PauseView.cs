using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class PauseView : BaseUIView
    {
        private const string CONTINUE_BUTTON_NAME = "ContinueButton";
        private const string RESTART_BUTTON_NAME = "RestartButton";
        private const string QUIT_BUTTON_NAME = "QuitButton";

        public new class UxmlFactory : UxmlFactory<PauseView> { }

        private Button m_continueBtn;
        private Button m_restartBtn;
        private Button m_quitButton;

        public PauseView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_continueBtn = this.Q<Button>(CONTINUE_BUTTON_NAME);
            if (m_continueBtn != null)
            {
                m_continueBtn.clicked += OnContinueButtonClicked;
            }

            m_restartBtn = this.Q<Button>(RESTART_BUTTON_NAME);
            if (m_restartBtn != null)
            {
                m_restartBtn.clicked += OnRestartButtonClicked;
            }

            m_quitButton = this.Q<Button>(QUIT_BUTTON_NAME);
            if (m_quitButton != null)
            {
                m_quitButton.clicked += OnQuitButtonClicked;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_continueBtn != null)
            {
                m_continueBtn.clicked -= OnContinueButtonClicked;
                m_continueBtn = null;
            }

            if (m_restartBtn != null)
            {
                m_restartBtn.clicked -= OnRestartButtonClicked;
                m_restartBtn = null;
            }
        }

        private void OnContinueButtonClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResumeGame();
            }
        }

        private void OnRestartButtonClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }

        private void OnQuitButtonClicked()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadMainMenu();
            }
        }
    }
}
