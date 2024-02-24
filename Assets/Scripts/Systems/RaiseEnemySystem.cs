using Inventory.Components;
using Inventory.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class RaiseEnemySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyDeadEvent>> _deadFilter = "events";
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsPoolInject<Unit> _unitsPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _deadFilter.Value)
            {
                var pool = _deadFilter.Pools.Inc1;
                ref var deadEvent = ref pool.Get(entity);

                var view = deadEvent.View;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var enemyEntity))
                {
                    continue;
                }
                
                ref var unit = ref _unitsPool.Value.Add(enemyEntity);
                
                unit.View = view;
                unit.Health = 100;
            }
        }
    }
}