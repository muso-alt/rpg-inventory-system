using Inventory.Components;
using Inventory.Data;
using Inventory.Services;
using Inventory.Tools;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class ItemsCreateSystem : IEcsInitSystem
    {
        private EcsCustomInject<InventoryService> _service;
        private EcsCustomInject<ItemsData> _itemsData;
        private EcsCustomInject<ObjectsPool<ItemView>> _objectPool;
        
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Init(IEcsSystems systems)
        {
            for (var index = 0; index < _itemsData.Value.ItemConfigs.Length; index++)
            {
                var config = _itemsData.Value.ItemConfigs[index];
                var entity = _defaultWorld.Value.NewEntity();

                var itemsPool = _defaultWorld.Value.GetPool<Item>();
                ref var item = ref itemsPool.Add(entity);

                var view = _objectPool.Value.GetItem();

                view.SetChild(Object.Instantiate(config.Icon));
                view.EcsEventWorld = _eventWorld.Value;
                view.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(entity);

                item.Name = config.ItemName;
                item.Type = config.Type;
                item.View = view;
                item.Weight = config.Weight;
                item.Icon = config.Icon;
                item.MaxStackSize = config.MaxStackSize;
                item.CurrentCount = config.MaxStackSize;

                ConfigureByType(config, entity);
                PutToCell(view, index);
            }
        }

        private void ConfigureByType(ItemConfig config, int entity)
        {
            switch (config.Type)
            {
                case ItemType.Ammo:
                {
                    ref var ammo = ref _defaultWorld.Value.GetByCreate<Ammo>(entity);
                    ammo.Damage = (int) config.Damage;
                    ammo.Type = config.AmmoType;
                    break;
                }
                case ItemType.BodyArmor:
                {
                    ref var bodyArmor = ref _defaultWorld.Value.GetByCreate<BodyArmor>(entity);
                    bodyArmor.Armor = config.Armor;
                    break;
                }
                case ItemType.MedKit:
                {
                    ref var medKit = ref _defaultWorld.Value.GetByCreate<MedKit>(entity);
                    medKit.HealingPower = config.HealingPower;
                    break;
                }
                case ItemType.HeadArmor:
                {
                    ref var medKit = ref _defaultWorld.Value.GetByCreate<HeadArmor>(entity);
                    medKit.Armor = config.HeadArmor;
                    break;
                }
            }
        }

        private void PutToCell(ItemView view, int index)
        {
            var cell = _service.Value.CellsView.Cells[index];
            
            view.SetParent(cell.Rect);
            cell.ChildItem = view;
        }
    }
}