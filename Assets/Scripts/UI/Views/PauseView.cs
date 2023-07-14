using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class PauseView : BaseUIView
    {
        private const string CONTINUE_BUTTON_NAME = "ContinueButton";

        public new class UxmlFactory : UxmlFactory<PauseView> { }

        private Button m_continueBtn;

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
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_continueBtn != null)
            {
                m_continueBtn.clicked -= OnContinueButtonClicked;
                m_continueBtn = null;
            }
        }

        private void OnContinueButtonClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
}
