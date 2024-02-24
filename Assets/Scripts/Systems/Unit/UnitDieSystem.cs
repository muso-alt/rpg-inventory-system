using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class UnitDieSystem : IEcsRunSystem
    {
        private EcsPoolInject<Unit> _unitsPool;
        private EcsFilterInject<Inc<Unit>> _unitFilter;
        
        private readonly EcsWorldInject _eventWorld = "events";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitFilter.Value)
            {
                ref var unit = ref _unitsPool.Value.Get(entity);

                if (unit.Health > 0)
                {
                    continue;
                }
                
                var unitDieEvent = new UnitDieEvent {View = unit.View};
                _eventWorld.Value.SendEvent(unitDieEvent);
            }
        }
    }
}