using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class UnitHealSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealEvent>> _healEvent = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _healEvent.Value)
            {
                var pool = _healEvent.Pools.Inc1;
                ref var healEvent = ref pool.Get(entity);

                var healerView = healEvent.ViewOfHealer;
                var healPower = healEvent.HealPower;

                if (!healerView.PackedEntityWithWorld.Unpack(out var world, out var unitEntity))
                {
                    continue;
                }
                
                var unitPool = world.GetPool<Unit>();
                ref var unit = ref unitPool.Get(unitEntity);

                unit.Health = Mathf.Clamp(unit.Health + healPower, 0, 100);
            }
        }
    }
}