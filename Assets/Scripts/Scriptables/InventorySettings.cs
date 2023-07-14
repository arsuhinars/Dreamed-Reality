using UnityEngine;

namespace DreamedReality.Scriptables
{
    [CreateAssetMenu(fileName = "InventorySettings", menuName = "Game/Controllers/Inventory Settings")]
    public class InventorySettings : ScriptableObject
    {
        public int maxItemsCount;
        public float beltRotationPeriod;
        public float beltItemsDistance;
        public float beltItemsScale;
    }
}
