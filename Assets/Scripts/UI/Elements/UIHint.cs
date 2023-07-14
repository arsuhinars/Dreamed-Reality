using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class UIHint : VisualElement
    {
        private const string PROMPT_TEXT_NAME = "PromptText";
        private const string HINT_TEXT_NAME = "HintText";

        private const string SHOWED_USS_CLASS = "hint-showed";
        private const string HIDDEN_USS_CLASS = "hint-hidden";

        public new class UxmlFactory : UxmlFactory<UIHint> { }

        private enum HintState
        {
            None, Showed, Hidden
        }

        public bool IsShowed => m_state == HintState.Showed;
        public string InputPrompt
        {
            get => m_inputPrompt;
            set => m_inputPrompt = value;
        }
        public string Text
        {
            get => m_text;
            set => m_text = value;
        }

        private HintState m_state = HintState.None;
        private string m_inputPrompt = string.Empty;
        private string m_text = string.Empty;

        private TextElement m_promptElement;
        private TextElement m_textElement;

        public void Show()
        {
            if (m_state == HintState.Showed)
            {
                return;
            }

            m_state = HintState.Showed;
            EnableInClassList(SHOWED_USS_CLASS, true);
            EnableInClassList(HIDDEN_USS_CLASS, false);

            m_promptElement.text = m_inputPrompt;
            m_textElement.text = m_text;
        }

        public void Hide()
        {
            if (m_state == HintState.Hidden)
            {
                return;
            }

            m_state = HintState.Hidden;
            EnableInClassList(SHOWED_USS_CLASS, false);
            EnableInClassList(HIDDEN_USS_CLASS, true);
        }

        public UIHint()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
            RegisterCallback<TransitionRunEvent>(OnTransitionRun);
            RegisterCallback<TransitionEndEvent>(OnTransitionEnd);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_promptElement = this.Q<Label>(PROMPT_TEXT_NAME);
            m_textElement = this.Q<Label>(HINT_TEXT_NAME);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            m_promptElement = null;
            m_textElement = null;
        }

        private void OnTransitionRun(TransitionRunEvent ev)
        {
            style.display = DisplayStyle.Flex;
        }

        private void OnTransitionEnd(TransitionEndEvent ev)
        {
            style.display = m_state == HintState.Showed ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
