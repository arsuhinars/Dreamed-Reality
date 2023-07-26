using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class NoteReadView : BaseUIView
    {
        private const string CLOSE_BUTTON_NAME = "CloseButton";
        private const string NOTE_TEXT_NAME = "NoteText";

        public new class UxmlFactory : UxmlFactory<NoteReadView> { }

        public LocalizedString Text
        {
            get => m_text;
            set
            {
                m_text = value;
                m_noteTextElement.LocalizedString = value;
            }
        }

        private LocalizedString m_text;
        private Button m_closeButton;
        private TextLocalizer m_noteTextElement;

        public NoteReadView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        protected override void OnShow()
        {
            if (GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
            {
                AudioManager.Instance.PlaySound(SfxType.NoteOpen);
            }
        }

        protected override void OnHide()
        {
            if (GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
            {
                AudioManager.Instance.PlaySound(SfxType.NoteClose);
            }
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_closeButton = this.Q<Button>(CLOSE_BUTTON_NAME);
            if (m_closeButton != null)
            {
                m_closeButton.clicked += OnCloseButtonClicked;
            }

            m_noteTextElement = this.Q<TextLocalizer>(NOTE_TEXT_NAME);
            if (m_noteTextElement != null)
            {
                m_noteTextElement.LocalizedString = m_text;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            if (m_closeButton != null)
            {
                m_closeButton.clicked -= OnCloseButtonClicked;
                m_closeButton = null;
            }

            m_noteTextElement = null;
        }

        private void OnCloseButtonClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
}
