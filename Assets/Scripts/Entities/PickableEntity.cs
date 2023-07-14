using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class PickableEntity : MonoBehaviour
    {
        public string ItemTag => m_itemTag;
        public bool IsPickedUp => m_isPickedUp;

        [SerializeField] private PickableSettings m_settings;
        [SerializeField] private string m_itemTag;
        [Space]
        [SerializeField] private Transform m_model;

        private bool m_isPickedUp = false;
        private Tween m_moveTween = null;
        private Tween m_rotationTween = null;

        public void Pickup()
        {
            m_isPickedUp = true;

            m_moveTween.Kill();
            m_rotationTween.Kill();
            m_model.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void Release()
        {
            Destroy(gameObject);
        }

        private void Start()
        {
            var moveTween = DOTween.Sequence();
            moveTween.Append(
                m_model.DOLocalMove(
                    m_settings.moveAmplitude, m_settings.movePeriod * 0.5f
                ).From(-m_settings.moveAmplitude).SetEase(Ease.InOutSine)
            );
            moveTween.Append(
                m_model.DOLocalMove(
                    -m_settings.moveAmplitude, m_settings.movePeriod * 0.5f
                ).From(m_settings.moveAmplitude).SetEase(Ease.InOutSine)
            );
            moveTween.SetLoops(-1);
            m_moveTween = moveTween;

            m_rotationTween = m_model.DOLocalRotate(
                new Vector3(0f, 360f, 0f),
                m_settings.rotationPeriod,
                RotateMode.FastBeyond360
            ).From(Vector3.zero).SetEase(Ease.Linear).SetLoops(-1);
        }
    }
}
