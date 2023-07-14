using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class ButtonEntity : UsableEntity
    {
        [Space]
        [SerializeField] private ButtonSettings m_settings;
        [SerializeField] private Transform m_buttonTransform;

        protected override void OnUse()
        {
            IsActive = false;

            float tweenDuration = m_settings.buttonMoveDuration * 0.5f;

            var sequence = DOTween.Sequence();
            sequence.Append(
                m_buttonTransform.DOLocalMove(
                    m_settings.buttonMoveOffset, tweenDuration
                ).From(Vector3.zero)
            );
            sequence.Append(
                m_buttonTransform.DOLocalMove(
                    Vector3.zero, tweenDuration
                ).From(m_settings.buttonMoveOffset)
            );

            sequence.OnComplete(() => IsActive = true);
        }
    }
}
