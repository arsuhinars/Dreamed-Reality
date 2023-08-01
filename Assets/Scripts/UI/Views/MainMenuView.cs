using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class MainMenuView : BaseUIView
    {
        private const string CONTINUE_BUTTON_NAME = "ContinueButton";
        private const string QUIT_BUTTON_NAME = "QuitButton";

        public new class UxmlFactory : UxmlFactory<MainMenuView> { }

        private Button m_continueBtn;
        private Button m_quitBtn;

        public MainMenuView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_continueBtn = this.Q<Button>(CONTINUE_BUTTON_NAME);
            m_quitBtn = this.Q<Button>(QUIT_BUTTON_NAME);

            if (m_continueBtn != null)
            {
                m_continueBtn.clicked += OnContinueButtonClicked;
                schedule.Execute(UpdateContinueButton).StartingIn(100);
            }

            if (m_quitBtn != null)
            {
                m_quitBtn.clicked += OnQuitButtonClicked;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_continueBtn != null)
            {
                m_continueBtn.clicked -= OnContinueButtonClicked;
                m_continueBtn = null;
            }

            if (m_quitBtn != null)
            {
                m_quitBtn.clicked -= OnQuitButtonClicked;
                m_quitBtn = null;
            }
        }

        private void UpdateContinueButton()
        {
            var progressManager = ProgressManager.Instance;
            if (progressManager != null && m_continueBtn != null)
            {
                m_continueBtn.SetEnabled(
                    progressManager.PlayerProgress.maxLevelIndex != -1
                );
            }
        }

        private void OnContinueButtonClicked()
        {
            var lvlIdx = ProgressManager.Instance.PlayerProgress.maxLevelIndex;
            LevelManager.Instance.LoadLevel((int)lvlIdx);
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
