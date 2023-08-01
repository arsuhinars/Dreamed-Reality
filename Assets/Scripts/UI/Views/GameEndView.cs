using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class GameEndView : BaseUIView
    {
        private const string QUIT_BUTTON_NAME = "QuitButton";

        public new class UxmlFactory : UxmlFactory<GameEndView> { }

        private Button m_quitButton;

        public GameEndView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_quitButton = this.Q<Button>(QUIT_BUTTON_NAME);
            if (m_quitButton != null)
            {
                m_quitButton.clicked += OnQuitButtonClick;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_quitButton != null)
            {
                m_quitButton.clicked -= OnQuitButtonClick;
                m_quitButton = null;
            }
        }

        private void OnQuitButtonClick()
        {
            LevelManager.Instance.LoadMainMenu();
        }
    }
}
