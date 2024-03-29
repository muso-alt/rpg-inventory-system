﻿using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class ItemQuantitySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ItemQuantityEvent>> _quantityFilter = "events";
        
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _quantityFilter.Value)
            {
                var pool = _quantityFilter.Pools.Inc1;
                ref var quantityEvent = ref pool.Get(entity);

                var view = quantityEvent.View;
                var quantity = quantityEvent.Quantity;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var id))
                {
                    continue;
                }

                var itemPool = world.GetPool<Item>();
                ref var item = ref itemPool.Get(id);

                item.CurrentCount = Mathf.Clamp(item.CurrentCount + quantity, 0, item.MaxStackSize);

                if (item.CurrentCount <= 0)
                {
                    var deleteEvent = new DeleteItemEvent {View = item.View};
                    _eventWorld.Value.SendEvent(deleteEvent);
                }
            }
        }
    }
}