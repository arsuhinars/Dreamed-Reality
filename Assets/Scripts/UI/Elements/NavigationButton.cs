using DreamedReality.Managers;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class NavigationButton : Button
    {
        public new class UxmlFactory : UxmlFactory<NavigationButton, UxmlTraits> { }

        public new class UxmlTraits : Button.UxmlTraits
        {
            public UxmlStringAttributeDescription m_TargetViewName = new()
            {
                name = "target-view-name"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var navigationButton = ve as NavigationButton;
                navigationButton.TargetViewName = m_TargetViewName.GetValueFromBag(bag, cc);
            }
        }

        public string TargetViewName
        {
            get => m_targetViewName;
            set => m_targetViewName = value;
        }

        private string m_targetViewName;

        public NavigationButton()
        {
            RegisterCallback<ClickEvent>(OnClick);
        }

        private void OnClick(ClickEvent ev)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetView(m_targetViewName);
            }
        }
    }
}
