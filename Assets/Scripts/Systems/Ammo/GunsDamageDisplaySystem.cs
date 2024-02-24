using Inventory.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class GunsDamageDisplaySystem : IEcsRunSystem
    {
        private EcsPoolInject<Gun> _gunsPool;
        private EcsFilterInject<Inc<Gun>> _gunFilter;
        
        private EcsPoolInject<Ammo> _ammoPool;
        private EcsFilterInject<Inc<Ammo>> _ammoFilter;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _gunFilter.Value)
            {
                ref var gun = ref _gunsPool.Value.Get(entity);

                var damage = GetDamage(ref gun);
                gun.View.SetDamage((int) damage);
            }
        }

        private float GetDamage(ref Gun gun)
        {
            foreach (var entity in _ammoFilter.Value)
            {
                ref var ammo = ref _ammoPool.Value.Get(entity);
                
                if (ammo.Type == gun.Type)
                {
                    return ammo.Damage;
                }
            }

            return 0;
        }
    }
}