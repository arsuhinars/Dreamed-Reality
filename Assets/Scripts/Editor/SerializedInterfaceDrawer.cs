using DreamedReality.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DreamedReality.Editor
{
    [CustomPropertyDrawer(typeof(SerializedInterface<>))]
    public class SerializedInterfaceDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var valueProperty = new PropertyField(property.FindPropertyRelative("m_value"))
            {
                label = property.displayName
            };

            return valueProperty;
        }
    }
}
