using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Inventory.Data
{
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
        private int _damage;

        [Space]
        [SerializeField, ShowIf(nameof(IsArmorSelected))]
        private int _armor;
        
        [Space]
        [SerializeField, ShowIf(nameof(IsMedKitSelected))]
        private int _healingPower;

        [SerializeField, ShowIf(nameof(IsMedKitSelected))]
        private int _spendAtOnce;
        
        [Space]
        [SerializeField, ShowIf(nameof(IsHeadArmorSelected))]
        private int _headArmor;

        public string ItemName => _itemName;
        public ItemType Type => _type;
        public AmmoType AmmoType => _ammoType;
        
        public GameObject Icon => _icon;
        
        public float Weight => _weight;
        public int Damage => _damage;
        public int Armor => _armor;
        public int HealingPower => _healingPower;
        public int SpendAtOnce => _spendAtOnce;
        public int HeadArmor => _headArmor;

        public int MaxStackSize => _maxStackSize;

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
    }
}