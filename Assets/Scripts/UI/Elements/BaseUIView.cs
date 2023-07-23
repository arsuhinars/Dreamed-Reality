using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class BaseUIView : VisualElement
    {
        private const string SHOWED_USS_CLASS = "view-showed";
        private const string HIDDEN_USS_CLASS = "view-hidden";

        public new class UxmlFactory : UxmlFactory<BaseUIView> { }

        private enum ViewState
        {
            None, Showed, Hidden
        }

        public bool IsShowed => m_state == ViewState.Showed;

        private ViewState m_state = ViewState.None;
        
        public void Show()
        {
            if (m_state == ViewState.Showed)
            {
                return;
            }

            m_state = ViewState.Showed;
            EnableInClassList(SHOWED_USS_CLASS, true);
            EnableInClassList(HIDDEN_USS_CLASS, false);
            UpdateInlineStyles();

            OnShow();
        }

        public void Hide()
        {
            if (m_state == ViewState.Hidden)
            {
                return;
            }

            m_state = ViewState.Hidden;
            EnableInClassList(SHOWED_USS_CLASS, false);
            EnableInClassList(HIDDEN_USS_CLASS, true);

            OnHide();
        }

        public BaseUIView()
        {
            RegisterCallback<TransitionRunEvent>(OnTransitionRun);
            RegisterCallback<TransitionEndEvent>(OnTransitionEnd);
            UpdateInlineStyles();
        }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        private void OnTransitionRun(TransitionRunEvent ev)
        {
            style.display = DisplayStyle.Flex;
        }

        private void OnTransitionEnd(TransitionEndEvent ev)
        {
            UpdateInlineStyles();
        }

        private void UpdateInlineStyles()
        {
            if (m_state == ViewState.Showed)
            {
                style.display = DisplayStyle.Flex;
                style.height = StyleKeyword.Initial;
            }
            else
            {
                style.display = DisplayStyle.None;
                style.height = 0f;
            }
        }
    }
}
