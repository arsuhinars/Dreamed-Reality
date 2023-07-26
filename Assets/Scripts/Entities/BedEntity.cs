using DreamedReality.Controllers;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace DreamedReality.Entities
{
    public class BedEntity : AbstractUsableEntity
    {
        public override LocalizedString UsageHintText => m_usageHintText;
        public override string RequiredItemTag => string.Empty;

        [SerializeField] private LocalizedString m_usageHintText;
        [Space]
        [Tooltip("Leave negative to disable this feature")]
        [SerializeField] private int m_targetLevelIndex = -1;
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private int m_targetSubLevelIndex;

        protected override void OnUse(GameObject user)
        {
            if (!user.TryGetComponent<PlayerController>(out var player))
            {
                return;
            }

            AudioManager.Instance.PlaySound(
                SfxType.GoingToBed, transform.position
            );
            TeleportPlayer(player);
        }

        private void Start()
        {
            IsActive = true;
        }

        private void TeleportPlayer(PlayerController player)
        {
            var screenFade = UIManager.Instance.ScreenFade;
            screenFade.FadeIn(OnFadeInComplete);

            player.Character.IsFreezed = true;

            void OnFadeInComplete()
            {
                if (m_targetLevelIndex < 0)
                {
                    LevelManager.Instance.SwitchSubLevel(m_targetSubLevelIndex);

                    player.TeleportTo(
                        m_targetTransform.position, m_targetTransform.rotation
                    );

                    screenFade.FadeOut(OnFadeOutComplete);
                }
                else
                {
                    LevelManager.Instance.LoadLevel(m_targetLevelIndex);
                }
            }

            void OnFadeOutComplete()
            {
                player.Character.IsFreezed = false;
            }
        }
    }
}
