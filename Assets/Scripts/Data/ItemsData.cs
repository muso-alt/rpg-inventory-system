using Inventory.Views;
using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(fileName = nameof(ItemsData), menuName = "Inventory/" + nameof(ItemsData), order = 0)]
    public class ItemsData : ScriptableObject
    {
        [SerializeField] private ItemView _itemView;
        [SerializeField] private ItemConfig[] _itemConfigs;

        public ItemView View => _itemView;
        public ItemConfig[] ItemConfigs => _itemConfigs;
    }
}