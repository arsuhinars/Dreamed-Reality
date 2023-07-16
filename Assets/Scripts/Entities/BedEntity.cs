using DreamedReality.Controllers;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class BedEntity : AbstractUsableEntity
    {
        public override string UsageHintText => m_usageHintText;
        public override string RequiredItemTag => string.Empty;

        [SerializeField] private string m_usageHintText;
        [Space]
        [SerializeField] private int m_bedLevelIndex;
        [SerializeField] private Transform m_teleportTransform;
        [SerializeField] private BedEntity m_targetBed;

        protected override void OnUse(GameObject user)
        {
            if (user.TryGetComponent<PlayerController>(out var player))
            {
                LevelManager.Instance.SwitchSubLevel(m_targetBed.m_bedLevelIndex);

                player.TeleportTo(
                    m_targetBed.m_teleportTransform.position,
                    m_targetBed.m_teleportTransform.rotation
                );
            }
        }

        private void Start()
        {
            IsActive = true;
        }
    }
}
