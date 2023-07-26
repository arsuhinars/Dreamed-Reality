using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class UIHint : VisualElement
    {
        private const string HINT_PROMPT_NAME = "HintPrompt";
        private const string HINT_TEXT_NAME = "HintText";

        private const string SHOWED_USS_CLASS = "hint-showed";
        private const string HIDDEN_USS_CLASS = "hint-hidden";

        private const string NONE_PROMPT_USS_CLASS = "hint__none-prompt";
        private const string USE_PROMPT_USS_CLASS = "hint__use-prompt";

        private const string HINT_TEXT_USS_CLASS = "hint__text";

        public new class UxmlFactory : UxmlFactory<UIHint> { }

        public enum PromptIconType
        {
            None, UsePrompt
        }

        private enum HintState
        {
            None, Showed, Hidden
        }

        public bool IsShowed => m_state == HintState.Showed;
        public PromptIconType PromptIcon
        {
            get => m_promptIcon;
            set
            {
                m_promptIcon = value;
                UpdateElements();
            }
        }
        public LocalizedString Text
        {
            get => m_text;
            set
            {
                m_text = value;
                UpdateElements();
            }
        }

        private PromptIconType m_promptIcon = PromptIconType.None;
        private LocalizedString m_text;

        private HintState m_state = HintState.None;
        private HintState m_nextState = HintState.None;
        private bool m_isTransitioning = false;
        
        private VisualElement m_promptElement;
        private TextLocalizer m_textElement;

        public void Show()
        {
            if (m_state == HintState.Showed)
            {
                return;
            }

            if (m_isTransitioning)
            {
                m_nextState = HintState.Showed;
                return;
            }

            m_state = HintState.Showed;
            m_isTransitioning = true;
            EnableInClassList(SHOWED_USS_CLASS, true);
            EnableInClassList(HIDDEN_USS_CLASS, false);
        }

        public void Hide()
        {
            if (m_state == HintState.Hidden)
            {
                return;
            }

            if (m_isTransitioning)
            {
                m_nextState = HintState.Hidden;
                return;
            }

            m_state = HintState.Hidden;
            m_isTransitioning = true;
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
            m_promptElement = this.Q<VisualElement>(HINT_PROMPT_NAME);
            m_textElement = this.Q<TextLocalizer>(HINT_TEXT_NAME);

            UpdateElements();

            Hide();
            m_isTransitioning = false;
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
            m_isTransitioning = false;
            style.display = m_state == HintState.Showed ? DisplayStyle.Flex : DisplayStyle.None;

            switch (m_nextState)
            {
                case HintState.Showed:
                    Show();
                    break;
                case HintState.Hidden:
                    Hide();
                    break;
            }
            m_nextState = HintState.None;
        }

        private void UpdateElements()
        {
            if (m_promptElement != null)
            {
                m_promptElement.EnableInClassList(NONE_PROMPT_USS_CLASS, false);
                m_promptElement.EnableInClassList(USE_PROMPT_USS_CLASS, false);

                var inputPromptUssClass = m_promptIcon switch
                {
                    PromptIconType.None => NONE_PROMPT_USS_CLASS,
                    PromptIconType.UsePrompt => USE_PROMPT_USS_CLASS,
                    _ => string.Empty,
                };
                m_promptElement.EnableInClassList(inputPromptUssClass, true);
            }

            if (m_textElement != null)
            {
                m_textElement.EnableInClassList(HINT_TEXT_USS_CLASS, true);
                m_textElement.LocalizedString = m_text;
            }
        }
    }
}
