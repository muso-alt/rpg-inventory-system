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
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _healEvent.Value)
            {
                var pool = _healEvent.Pools.Inc1;
                ref var healEvent = ref pool.Get(entity);

                var healerView = healEvent.ViewOfHealer;
                var medKitView = healEvent.ViewOfMedKit;

                if (!healerView.PackedEntityWithWorld.Unpack(out var world, out var unitEntity))
                {
                    continue;
                }
                
                var unitPool = world.GetPool<Unit>();
                ref var unit = ref unitPool.Get(unitEntity);

                //TODO: Set max health system in cmp
                if (unit.Health >= 100)
                {
                    continue;
                }
                
                HealUnit(medKitView, ref unit);
            }
        }

        private void HealUnit(ItemView view, ref Unit unit)
        {
            if (!view.PackedEntityWithWorld.Unpack(out var world, out var entity))
            {
                return;
            }
            
            var medKitPool = world.GetPool<MedKit>();
            ref var medKit = ref medKitPool.Get(entity);

            unit.Health = Mathf.Clamp(unit.Health + medKit.HealingPower, 0, 100);
            
            var quantityEvent = new ItemQuantityEvent {View = view, Quantity = -medKit.SpendAtOnce};
            _eventWorld.Value.SendEvent(quantityEvent);
        }
    }
}