using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class ShootingSystem : IEcsRunSystem
    {
        private EcsPoolInject<Item> _itemsPool;
        private EcsPoolInject<Ammo> _ammoPool;
        private EcsFilterInject<Inc<Ammo>> _ammoFilter;
        
        private readonly EcsWorldInject _eventWorld = "events";
        
        private readonly EcsFilterInject<Inc<GunTriggerEvent>> _gunTriggerFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _gunTriggerFilter.Value)
            {
                var pool = _gunTriggerFilter.Pools.Inc1;
                ref var shotEvent = ref pool.Get(entity);

                var view = shotEvent.GunView;
                
                if (!view.PackedEntityWithWorld.Unpack(out var world, out var itemEntity))
                {
                    continue;
                }

                var gunPool = world.GetPool<Gun>();
                ref var gun = ref gunPool.Get(itemEntity);

                TryShoot(ref gun);
            }
        }

        private void TryShoot(ref Gun gun)
        {
            if (_ammoPool.TryGetAmmoEntityByType(_ammoFilter, gun.Type, out var entity))
            {
                Shoot(entity, ref gun);
                return;
            }

            Debug.Log("Can't find bullets in inventory");
        }


        private void Shoot(int entity, ref Gun gun)
        {
            ref var item = ref _itemsPool.Value.Get(entity);
            ref var ammo = ref _ammoPool.Value.Get(entity);
            
            if (item.CurrentCount < gun.BulletSpendCount)
            {
                Debug.Log("You don't have bullets");
                return;
            }
            
            var quantityEvent = new ItemQuantityEvent {View = item.View, Quantity = -gun.BulletSpendCount};
            _eventWorld.Value.SendEvent(quantityEvent);

            var shotType = (ShootType) Random.Range(0, 2);

            var playerShotEvent = new PlayerShootEvent {Damage = ammo.Damage, ShootType = shotType};
            _eventWorld.Value.SendEvent(playerShotEvent);
        }
    }
}