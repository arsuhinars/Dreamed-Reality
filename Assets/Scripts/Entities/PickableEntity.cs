using DG.Tweening;
using DreamedReality.Managers;
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

        private Transform m_initialParent;
        private Vector3 m_initialPosition;
        private Quaternion m_initialRotation;
        
        private Tween m_moveTween = null;
        private Tween m_rotationTween = null;

        public void Pickup()
        {
            m_isPickedUp = true;

            KillTweens();

            m_model.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void Release()
        {
            ResetState();
            gameObject.SetActive(false);
        }

        private void Start()
        {
            m_initialParent = transform.parent;
            m_initialPosition = transform.position;
            m_initialRotation = transform.rotation;

            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnEnable()
        {
            CreateTweens();
        }

        private void OnDisable()
        {
            KillTweens();
        }

        private void OnGameStart()
        {
            ResetState();
            KillTweens();
            CreateTweens();
        }

        private void CreateTweens()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

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

        private void KillTweens()
        {
            m_moveTween?.Kill();
            m_rotationTween?.Kill();

            m_moveTween = null;
            m_rotationTween = null;
        }

        private void ResetState()
        {
            m_isPickedUp = false;

            gameObject.SetActive(true);

            transform.SetParent(m_initialParent);
            transform.localScale = Vector3.one;
            transform.SetPositionAndRotation(m_initialPosition, m_initialRotation);
        }
    }
}
