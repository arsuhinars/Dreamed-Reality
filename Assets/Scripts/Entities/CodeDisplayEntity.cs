using DG.Tweening;
using DreamedReality.Managers;
using DreamedReality.Scriptables;
using UnityEngine;
using UnityEngine.Localization;

namespace DreamedReality.Entities
{
    public class CodeDisplayEntity : AbstractUsableEntity
    {
        public int CurrentCode => m_currCode;
        public override LocalizedString UsageHintText => m_usageHintText;
        public override string RequiredItemTag => string.Empty;

        [SerializeField] private LocalizedString m_usageHintText;
        [Space]
        [SerializeField] private int m_initialCode;
        [SerializeField] private CodeDisplaySettings m_settings;
        [SerializeField] private MeshRenderer m_displayModel;

        private int m_offsetPropertyId;
        private Tween m_tween;
        private int m_currCode;

        protected override void OnUse(GameObject user)
        {
            int nextCode = m_currCode - m_settings.minCodeNumber + 1;
            nextCode %= m_settings.maxCodeNumber - m_settings.minCodeNumber + 1;
            nextCode += m_settings.minCodeNumber;
            m_currCode = nextCode;

            PlayTween();
            AudioManager.Instance.PlaySound(m_settings.usingSfx, transform.position);
        }

        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;

            IsActive = true;
            m_offsetPropertyId = Shader.PropertyToID("_SliceOffset");
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnDisable()
        {
            m_tween?.Complete();
            m_tween?.Kill();
            m_tween = null;
        }

        private void OnGameStart()
        {
            m_currCode = m_initialCode;
            PlayTween();
            m_tween.Complete();
            m_tween = null;
        }

        private void PlayTween()
        {
            m_tween?.Kill();

            var tween = m_displayModel.material.DOVector(
                new Vector2(0f, m_currCode),
                m_offsetPropertyId,
                m_settings.numberChangeDuration
            );
            if (m_currCode == m_settings.minCodeNumber)
            {
                tween.From(new Vector2(0f, m_currCode - 1f));
            }

            m_tween = tween;
        }
    }
}
