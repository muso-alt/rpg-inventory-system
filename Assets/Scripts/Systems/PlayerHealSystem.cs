using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
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

                ref var unit = ref _unitPool.Value.Get(playerEntity);

                var unitHealEvent = new HealEvent {ViewOfHealer = unit.View, ViewOfMedKit = view};
                _eventWorld.Value.SendEvent(unitHealEvent);
            }
        }
    }
}