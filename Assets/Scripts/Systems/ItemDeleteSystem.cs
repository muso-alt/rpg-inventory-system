using Inventory.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class ItemDeleteSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DeleteItemEvent>> _deleteEvent = "events";
        
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

                Object.Destroy(view.gameObject);
                world.DelEntity(itemEntity);
            }
        }
    }
}