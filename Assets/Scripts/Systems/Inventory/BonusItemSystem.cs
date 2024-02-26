using System.Collections.Generic;
using System.Linq;
using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Inventory.Tools;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class BonusItemSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<InventoryService> _inventoryService;
        private readonly EcsCustomInject<DeletedItemsPool> _deletedItems;
        private readonly EcsCustomInject<ObjectsPool<ItemView>> _objPool;
        private readonly EcsCustomInject<ItemPlaceService> _placeService;
        
        private readonly EcsFilterInject<Inc<EnemyDeadEvent>> _deadFilter = "events";
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsPoolInject<Item> _itemPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var unused in _deadFilter.Value)
            {
                if (!_deletedItems.Value.TryGetRandomItem(out var itemEntity))
                {
                    continue;
                }

                var itemView = _objPool.Value.GetItem();
                ref var item = ref _itemPool.Value.Get(itemEntity);

                item.CurrentCount = item.MaxStackSize;
                
                itemView.EcsEventWorld = _eventWorld.Value;
                itemView.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(itemEntity);
                itemView.SetChild(Object.Instantiate(item.Icon));

                item.View = itemView;

                var cell = _inventoryService.Value.CellsView.Cells.GetRandomEmptyCell();
                
                _placeService.Value.TryPutItemToCell(itemView, cell);
            }
        }
    }
}