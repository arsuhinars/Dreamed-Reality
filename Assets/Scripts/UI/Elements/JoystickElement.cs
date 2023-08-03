using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class JoystickElement : VisualElement
    {
        private const string JOYSTICK_USS_CLASS = "joystick";
        private const string JOYSTICK_POINTER_USS_CLASS = "joystick__pointer";

        public new class UxmlFactory : UxmlFactory<JoystickElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlIntAttributeDescription m_JoystickRadius = new()
            {
                name = "joystick-radius"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var joystick = ve as JoystickElement;

                joystick.JoystickRadius = m_JoystickRadius.GetValueFromBag(bag, cc);
            }
        }

        public event Action<Vector2> OnUpdate;

        public int JoystickRadius
        {
            get => m_joystickRadius;
            set => m_joystickRadius = value;
        }

        private int m_joystickRadius;
        private bool m_isDragging = false;
        private int m_activePointerId;
        private VisualElement m_joystickPointer;

        public JoystickElement()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            AddToClassList(JOYSTICK_USS_CLASS);

            m_joystickPointer = new VisualElement();
            m_joystickPointer.AddToClassList(JOYSTICK_POINTER_USS_CLASS);
            m_joystickPointer.pickingMode = PickingMode.Ignore;

            Add(m_joystickPointer);

            var root = ev.destinationPanel.visualTree;

            root.RegisterCallback<PointerUpEvent>(OnPointerUp);
            root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            RemoveFromClassList(JOYSTICK_USS_CLASS);
            Remove(m_joystickPointer);

            var root = ev.originPanel.visualTree;

            root.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            root.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        }

        private void OnPointerDown(PointerDownEvent ev)
        {
            m_isDragging = true;
            m_activePointerId = ev.pointerId;
            UpdateJoystick(ev.position);
        }

        private void OnPointerUp(PointerUpEvent ev)
        {
            if (m_isDragging && ev.pointerId == m_activePointerId)
            {
                m_isDragging = false;
                UpdateJoystick(worldBound.center);
            }
        }

        private void OnPointerMove(PointerMoveEvent ev)
        {
            if (m_isDragging && ev.pointerId == m_activePointerId)
            {
                UpdateJoystick(ev.position);
            }
        }

        private void UpdateJoystick(Vector2 pointerPosition)
        {
            var offset = pointerPosition - worldBound.center;

            float offsetLength = offset.magnitude;
            if (offsetLength > m_joystickRadius)
            {
                offset *= m_joystickRadius / offsetLength;
            }

            OnUpdate?.Invoke(
                new Vector2(
                    offset.x / m_joystickRadius,
                    -offset.y / m_joystickRadius
                )
            );

            m_joystickPointer.style.translate = new Translate(offset.x, offset.y, 0f);
        }
    }
}
