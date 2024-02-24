using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class DeadUnitHandleSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitDieEvent>> _unitDieFilter = "events";
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitDieFilter.Value)
            {
                var pool = _unitDieFilter.Pools.Inc1;
                ref var dieEvent = ref pool.Get(entity);

                var view = dieEvent.View;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var unitEntity))
                {
                    continue;
                }

                var playerPool = world.GetPool<Player>();
                var enemyPool = world.GetPool<Enemy>();
                
                if (playerPool.Has(unitEntity))
                {
                    SendEvent(new PlayerDeadEvent {View = view});
                }
                else if (enemyPool.Has(unitEntity))
                {
                    SendEvent(new EnemyDeadEvent {View = view});
                }
            }
        }

        private void SendEvent<T>(T deadEvent) where T : struct
        {
            _eventWorld.Value.SendEvent(deadEvent);
        }
    }
}