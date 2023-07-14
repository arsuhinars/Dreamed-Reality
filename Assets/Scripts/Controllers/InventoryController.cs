using DreamedReality.Entities;
using DreamedReality.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace DreamedReality.Controllers
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySettings m_settings;
        [Space]
        [SerializeField] private Transform m_beltTransform;

        private Dictionary<string, PickableEntity> m_inventory = new();

        public bool TryAddItem(PickableEntity entity)
        {
            if (
                m_inventory.Count >= m_settings.maxItemsCount
                || m_inventory.ContainsKey(entity.ItemTag)
            ) {
                return false;
            }

            entity.Pickup();

            entity.transform.SetParent(m_beltTransform);
            entity.transform.localRotation = Quaternion.identity;
            entity.transform.localScale = Vector3.one * m_settings.beltItemsScale;

            m_inventory.Add(entity.ItemTag, entity);

            return true;
        }

        public bool HasItem(string tag)
        {
            return m_inventory.ContainsKey(tag);
        }

        public bool ReleaseItem(string tag)
        {
            if (m_inventory.TryGetValue(tag, out var entity))
            {
                entity.Release();
                m_inventory.Remove(tag);
                return true;
            }

            return false;
        }

        private void Update()
        {
            float angleStep = 2f * Mathf.PI / (m_inventory.Count + 1);
            int counter = 0;
            foreach (var entity in m_inventory.Values)
            {
                float angle = angleStep * counter;
                angle += 2f * Mathf.PI * Time.time / m_settings.beltRotationPeriod;

                entity.transform.localPosition = new Vector3(
                    Mathf.Cos(angle), 0f, Mathf.Sin(angle)
                ) * m_settings.beltItemsDistance;

                ++counter;
            }
        }
    }
}
