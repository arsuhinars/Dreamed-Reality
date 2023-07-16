using DreamedReality.Entities;
using DreamedReality.Managers;
using DreamedReality.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySettings m_settings;
        [Space]
        [SerializeField] private Transform m_beltTransform;

        private Dictionary<string, PickableEntity> m_items = new();

        public bool HasItem(string tag)
        {
            return m_items.ContainsKey(tag);
        }

        public bool ReleaseItem(string tag)
        {
            if (m_items.TryGetValue(tag, out var entity))
            {
                entity.Release();
                m_items.Remove(tag);
                return true;
            }

            return false;
        }

        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void Update()
        {
            float angleStep = 2f * Mathf.PI / (m_items.Count + 1);
            int counter = 0;
            foreach (var entity in m_items.Values)
            {
                float angle = angleStep * counter;
                angle += 2f * Mathf.PI * Time.time / m_settings.beltRotationPeriod;

                entity.transform.localPosition = new Vector3(
                    Mathf.Cos(angle), 0f, Mathf.Sin(angle)
                ) * m_settings.beltItemsDistance;

                ++counter;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PickableEntity>(out var entity))
            {
                TryAddItem(entity);
            }
        }

        private void OnGameStart()
        {
            m_items.Clear();
        }

        private bool TryAddItem(PickableEntity entity)
        {
            if (m_items.Count >= m_settings.maxItemsCount || m_items.ContainsKey(entity.ItemTag))
            {
                return false;
            }

            entity.Pickup();

            entity.transform.SetParent(m_beltTransform);
            entity.transform.localRotation = Quaternion.identity;
            entity.transform.localScale = Vector3.one * m_settings.beltItemsScale;

            m_items.Add(entity.ItemTag, entity);

            return true;
        }
    }
}
