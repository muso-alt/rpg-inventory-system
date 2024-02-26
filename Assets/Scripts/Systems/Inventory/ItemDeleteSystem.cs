using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Inventory.Tools;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class ItemDeleteSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<Item> _itemPool;
        private readonly EcsFilterInject<Inc<DeleteItemEvent>> _deleteEvent = "events";
        
        private readonly EcsCustomInject<DeletedItemsPool> _deletedItems;
        private readonly EcsCustomInject<ObjectsPool<ItemView>> _objPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _deleteEvent.Value)
            {
                var pool = _deleteEvent.Pools.Inc1;
                ref var deleteEvent = ref pool.Get(entity);

                var view = deleteEvent.View;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var itemEntity))
                {
                    continue;
                }

                _objPool.Value.Return(view);
                
                _deletedItems.Value.AddToDeleted(itemEntity);
            }
        }
    }
}