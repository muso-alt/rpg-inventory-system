using System;
using Inventory.Views;
using Sirenix.OdinInspector;
using TMPro;
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

    [Serializable]
    public struct ItemConfig
    {
        [SerializeField] private string _itemName;
        [SerializeField] private ItemType _type;
        [SerializeField] private int _maxStackSize;

        [Space]
        
        [SerializeField] private GameObject _icon;
        [SerializeField] private float _weight;
        
        [Space]
        [SerializeField, ShowIf(nameof(IsAmmoSelected))]
        private AmmoType _ammoType;

        [Space]
        [SerializeField, ShowIf(nameof(IsAmmoSelected))]
        private float _damage;

        [Space]
        [SerializeField, ShowIf(nameof(IsArmorSelected))]
        private float _armor;
        
        [Space]
        [SerializeField, ShowIf(nameof(IsMedKitSelected))]
        private float _healingPower;
        
        [Space]
        [SerializeField, ShowIf(nameof(IsHeadArmorSelected))]
        private float _headArmor;

        public string ItemName => _itemName;
        public ItemType Type => _type;
        public AmmoType AmmoType => _ammoType;
        
        public GameObject Icon => _icon;
        
        public float Weight => _weight;
        public float Damage => _damage;
        public float Armor => _armor;
        public float HealingPower => _healingPower;
        public float HeadArmor => _headArmor;

        public int MaxStackSize => _maxStackSize;

#if UNITY_EDITOR
        private bool IsAmmoSelected()
        {
            return _type == ItemType.Ammo;
        }
        
        private bool IsArmorSelected()
        {
            return _type == ItemType.BodyArmor;
        }
        
        private bool IsHeadArmorSelected()
        {
            return _type == ItemType.HeadArmor;
        }

        private bool IsMedKitSelected()
        {
            return _type == ItemType.MedKit;
        }
#endif
    }
}