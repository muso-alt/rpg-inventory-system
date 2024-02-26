using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Inventory.Tools;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class ItemsCreateSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<InventoryService> _service;
        private readonly EcsCustomInject<ItemsData> _itemsData;
        private readonly EcsCustomInject<ObjectsPool<ItemView>> _objectPool;

        private readonly EcsFilterInject<Inc<CreateItemEvent>> _itemsFilter = "events";
        
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _itemsFilter.Value)
            {
                var pool = _itemsFilter.Pools.Inc1;
                var itemEvent = pool.Get(entity);

                pool.Del(entity);

                foreach (var itemData in itemEvent.Items)
                {
                    CreateItem(itemData);
                }
            }
        }

        private void CreateItem(ItemData itemData)
        {
            var config = GetItemConfigByName(itemData.Name);
            
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
            item.CurrentCount = itemData.CurrentCount;

            ConfigureByType(config, entity);

            var cells = _service.Value.CellsView.Cells;
            var cell = cells[itemData.CellIndex];

            PutToCell(view, cell);
        }

        private ItemConfig GetItemConfigByName(string name)
        {
            foreach (var itemConfig in _itemsData.Value.ItemConfigs)
            {
                if (itemConfig.ItemName.Equals(name))
                {
                    return itemConfig;
                }
            }

            Debug.LogError("Can't find");
            return default;
        }
        
        private void ConfigureByType(ItemConfig config, int entity)
        {
            switch (config.Type)
            {
                case ItemType.Ammo:
                {
                    ref var ammo = ref _defaultWorld.Value.GetByCreate<Ammo>(entity);
                    ammo.Damage = config.Damage;
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
                    medKit.SpendAtOnce = config.SpendAtOnce;
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

        private void PutToCell(ItemView view, CellView cell)
        {
            var placeItemEvent = new PlaceItemEvent {View = view, Cell = cell};
            _eventWorld.Value.SendEvent(placeItemEvent);
        }
    }
}