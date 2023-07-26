using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class TextLocalizer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TextLocalizer, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlStringAttributeDescription m_TableName = new()
            {
                name = "table-name"
            };
            public UxmlStringAttributeDescription m_EntryName = new()
            {
                name = "entry-name"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var textLocalizer = ve as TextLocalizer;

                textLocalizer.TableName = m_TableName.GetValueFromBag(bag, cc);
                textLocalizer.EntryName = m_EntryName.GetValueFromBag(bag, cc);
            }
        }

        public LocalizedString LocalizedString
        {
            get => m_localizedString;
            set
            {
                if (m_localizedString != null)
                {
                    m_localizedString.StringChanged -= UpdateText;
                }

                m_localizedString = value;
                if (value != null)
                {
                    value.StringChanged += UpdateText;
                    value.RefreshString();

                    m_tableName = value.TableReference;
                    m_entryName = value.TableEntryReference;
                }
                else
                {
                    UpdateText(string.Empty);

                    m_tableName = string.Empty;
                    m_entryName = string.Empty;
                }
            }
        }
        private string TableName
        {
            get => m_tableName;
            set => m_tableName = value;
        }
        private string EntryName
        {
            get => m_entryName;
            set => m_entryName = value;
        }

        private string m_tableName = string.Empty;
        private string m_entryName = string.Empty;
        private LocalizedString m_localizedString;

        private TextElement m_textElement = null;

        public TextLocalizer()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            foreach (var el in Children())
            {
                m_textElement = el as TextElement;
                break;
            }

            UpdateLocalizedString();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            m_textElement = null;
            if (m_localizedString != null)
            {
                m_localizedString.StringChanged -= UpdateText;
                m_localizedString = null;
            }
        }

        private void UpdateLocalizedString()
        {
            LocalizedString = new LocalizedString(m_tableName, m_entryName);
        }

        private void UpdateText(string s)
        {
            if (m_textElement != null)
            {
                m_textElement.text = s;
            }
        }
    }
}
