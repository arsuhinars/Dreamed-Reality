using DreamedReality.UI.Elements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamedReality.Managers
{
    public class UIManager : MonoBehaviour
    {
        private const string VIEWS_CONTAINER_NAME = "ViewsContainer";
        private const string HINT_ELEMENT_NAME = "GameHint";
        private const string FADE_ELEMENT_NAME = "ScreenFade";

        private const string USS_SFX_CLICK_CLASS = "play-click-sfx";
        private const string USS_SFX_SWITCH_CLASS = "play-switch-sfx";

        public static UIManager Instance { get; private set; } = null;

        public FadeElement ScreenFade => m_screenFade;
        public UIHint UIHint => m_hintElement;

        [SerializeField] private UIDocument m_document;
        [SerializeField] private string m_initialViewName;

        private KeyValuePair<string, BaseUIView> m_currView = new("", null);
        private Dictionary<string, BaseUIView> m_viewsByName;
        private FadeElement m_screenFade;
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

            if (!string.IsNullOrEmpty(m_initialViewName))
            {
                SetView(m_initialViewName);
            }
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
            var root = m_document.rootVisualElement;

            m_viewsByName = new();

            var viewsContainer = root.Q(VIEWS_CONTAINER_NAME);
            foreach (var child in viewsContainer.Children())
            {
                if (child is not BaseUIView view)
                {
                    Debug.LogWarning("UI views in UIDocument must be inherited from BaseUIView");
                    return;
                }

                m_viewsByName.Add(view.name, view);
            }

            m_hintElement = root.Q<UIHint>(HINT_ELEMENT_NAME);
            m_screenFade = root.Q<FadeElement>(FADE_ELEMENT_NAME);
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
