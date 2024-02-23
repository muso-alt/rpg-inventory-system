using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class GunsInitSystem : IEcsInitSystem
    {
        private EcsCustomInject<InventoryService> _service;
        
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsWorldInject _defaultWorld = default;
        
        public void Init(IEcsSystems systems)
        {
            var pistolView = _service.Value.Guns.PistolView;
            var riffleView = _service.Value.Guns.RiffleView;
            
            CreateEntity(pistolView, AmmoType.Pistol);
            CreateEntity(riffleView, AmmoType.Riffle);
            
            GunChoose(pistolView.Toggle.isOn, pistolView);
            GunChoose(riffleView.Toggle.isOn, riffleView);
        }

        private void CreateEntity(GunView view, AmmoType type)
        {
            var entity = _defaultWorld.Value.NewEntity();

            ref var gun = ref _defaultWorld.Value.GetByCreate<Gun>(entity);
            
            view.Toggle.onValueChanged.RemoveAllListeners();
            view.Toggle.onValueChanged.AddListener(toggle => GunChoose(toggle, view));

            view.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(entity);
            
            gun.Type = type;
            gun.BulletSpendCount = view.BulletSpendCount;
            gun.View = view;
        }

        private void GunChoose(bool toggle, GunView view)
        {
            if (toggle)
            {
                SendTypeChangeEvent(view);
            }
        }

        private void SendTypeChangeEvent(GunView view)
        {
            var gunEvent = new ActiveGunEvent {View = view};
            _eventWorld.Value.SendEvent(gunEvent);
        }
    }
}