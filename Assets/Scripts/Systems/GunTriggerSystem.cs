using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class GunTriggerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<InventoryService> _service;
        private readonly EcsWorldInject _eventWorld = "events";
        
        private readonly EcsFilterInject<Inc<ActiveGunEvent>> _gunFilter = "events";

        private GunView _gunView;
        
        public void Init(IEcsSystems systems)
        {
            _service.Value.Guns.ShotButton.onClick.RemoveAllListeners();
            _service.Value.Guns.ShotButton.onClick.AddListener(Shot);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _gunFilter.Value)
            {
                var pool = _gunFilter.Pools.Inc1;
                ref var gunEvent = ref pool.Get(entity);
                _gunView = gunEvent.View;
            }
        }
        
        private void Shot()
        {
            var shotEvent = new GunTriggerEvent {GunView = _gunView};
            _eventWorld.Value.SendEvent(shotEvent);
        }
    }
}