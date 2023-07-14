using DreamedReality.UI.Elements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamedReality.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; } = null;

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

        public void HideAllViews()
        {
            foreach (var view in m_viewsByName.Values)
            {
                view.Hide();
            }
        }

        public void ShowHint(string inputPrompt, string text)
        {
            m_hintElement.InputPrompt = inputPrompt;
            m_hintElement.Text = text;
            m_hintElement.Show();
        }

        public void HideHint()
        {
            m_hintElement.Hide();
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
            HideAllViews();
            HideHint();
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
    }
}
