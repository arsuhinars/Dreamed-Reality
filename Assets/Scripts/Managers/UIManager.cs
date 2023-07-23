using DreamedReality.UI.Elements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace DreamedReality.Managers
{
    public class UIManager : MonoBehaviour
    {
        private const string USS_SFX_CLICK_CLASS = "play-click-sfx";
        private const string USS_SFX_SWITCH_CLASS = "play-switch-sfx";

        public static UIManager Instance { get; private set; } = null;

        public UIHint UIHint => m_hintElement;

        [SerializeField] private UIDocument m_document;
        [SerializeField] private string m_viewsContainerName;
        [SerializeField] private string m_hintElementName;

        private KeyValuePair<string, BaseUIView> m_currView = new("", null);
        private Dictionary<string, BaseUIView> m_viewsByName;
        private UIHint m_hintElement;

        public void SetView(string viewName)
        {
            if (!m_viewsByName.TryGetValue(viewName, out var view))
            {
                Debug.LogWarning($"UI view with name \"{viewName}\" doesn't exist");
                return;
            }

            m_currView.Value?.Hide();
            m_currView = new(viewName, view);

            view.Show();
        }

        public BaseUIView GetView(string viewName)
        {
            if (m_viewsByName.TryGetValue(viewName, out var view))
            {
                return view;
            }

            return null;
        }

        public void HideAllViews()
        {
            foreach (var view in m_viewsByName.Values)
            {
                view.Hide();
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Singleton already exists");
                Destroy(this);
            }
        }

        private void Start()
        {
            FindElements();
            AddUISoundHandlers();
            HideAllViews();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
        
        private void FindElements()
        {
            m_viewsByName = new();

            var viewsContainer = m_document.rootVisualElement.Q(m_viewsContainerName);
            foreach (var child in viewsContainer.Children())
            {
                if (child is not BaseUIView view)
                {
                    Debug.LogWarning("UI views in UIDocument must be inherited from BaseUIView");
                    return;
                }

                m_viewsByName.Add(view.name, view);
            }

            m_hintElement = m_document.rootVisualElement.Q<UIHint>(m_hintElementName);
        }

        private void AddUISoundHandlers()
        {
            var root = m_document.rootVisualElement;

            var clickElements = root.Query(className: USS_SFX_CLICK_CLASS).Build();
            foreach (var el in clickElements)
            {
                el.RegisterCallback<ClickEvent>(PlayClickSound);
            }

            var switchElements = root.Query(className: USS_SFX_SWITCH_CLASS).Build();
            foreach (var el in switchElements)
            {
                el.RegisterCallback<ClickEvent>(PlaySwitchSound);
            }
        }

        private void PlayClickSound(ClickEvent ev)
        {
            AudioManager.Instance.PlaySound(SfxType.UIClick);
        }

        private void PlaySwitchSound(ClickEvent ev)
        {
            AudioManager.Instance.PlaySound(SfxType.UISwitch);
        }
    }
}
