using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class ShotSystem : IEcsRunSystem
    {
        private EcsPoolInject<Item> _itemsPool;
        private EcsPoolInject<Ammo> _ammoPool;
        private EcsFilterInject<Inc<Ammo>> _ammoFilter;
        
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsWorldInject _defaultWorld = default;
        
        private readonly EcsFilterInject<Inc<GunTriggerEvent>> _shotFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _shotFilter.Value)
            {
                var pool = _shotFilter.Pools.Inc1;
                ref var shotEvent = ref pool.Get(entity);

                var view = shotEvent.GunView;
                
                if (!view.PackedEntityWithWorld.Unpack(out var world, out var itemEntity))
                {
                    continue;
                }

                var gunPool = world.GetPool<Gun>();
                ref var gun = ref gunPool.Get(itemEntity);

                TryShot(ref gun);
            }
        }

        private void TryShot(ref Gun gun)
        {
            if (_ammoPool.TryGetAmmoEntityByType(_ammoFilter, gun.Type, out var entity))
            {
                ref var item = ref _itemsPool.Value.Get(entity);
                
                if (item.CurrentCount < gun.BulletSpendCount)
                {
                    Debug.Log("You don't have bullets");
                    return;
                }
                
                Shot(ref item, ref gun);
                return;
            }
            
            
            Debug.Log("Can't find bullets in inventory");
        }


        private void Shot(ref Item item, ref Gun gun)
        {
            Debug.Log("Yes");
            var quantityEvent = new ItemQuantityEvent {View = item.View, Quantity = -gun.BulletSpendCount};
            _eventWorld.Value.SendEvent(quantityEvent);
        }
    }
}