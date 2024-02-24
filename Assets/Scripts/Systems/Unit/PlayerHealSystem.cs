using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class PlayerHealSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<Unit> _unitPool;
        private readonly EcsPoolInject<MedKit> _medKitPool;
        private readonly EcsFilterInject<Inc<Player>> _playerFilter;
        
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsFilterInject<Inc<PlayerHealEvent>> _healEvent = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _healEvent.Value)
            {
                var pool = _healEvent.Pools.Inc1;
                ref var healEvent = ref pool.Get(entity);

                var view = healEvent.View;

                if (!_playerFilter.TryGetFirstEntityFromFilter(out var playerEntity))
                {
                    continue;
                }
                
                HealUnit(view, playerEntity);
            }
        }
        
        private void HealUnit(ItemView view, int playerEntity)
        {
            ref var unit = ref _unitPool.Value.Get(playerEntity);

            if (unit.Health >= 100)
            {
                return;
            }
            
            if (!view.PackedEntityWithWorld.Unpack(out var world, out var entity))
            {
                return;
            }
            
            var medKitPool = world.GetPool<MedKit>();
            ref var medKit = ref medKitPool.Get(entity);
            
            var unitHealEvent = new HealEvent {ViewOfHealer = unit.View, HealPower = medKit.HealingPower};
            _eventWorld.Value.SendEvent(unitHealEvent);
            
            var quantityEvent = new ItemQuantityEvent {View = view, Quantity = -medKit.SpendAtOnce};
            _eventWorld.Value.SendEvent(quantityEvent);
        }
    }
}