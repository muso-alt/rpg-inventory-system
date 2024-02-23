using System;
using Inventory.Components;
using Inventory.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class BulletHitSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BulletHitTargetEvent>> _bulletHitFilter = "events";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bulletHitFilter.Value)
            {
                var pool = _bulletHitFilter.Pools.Inc1;
                ref var shotEvent = ref pool.Get(entity);

                var view = shotEvent.View;
                var shotType = shotEvent.ShootType;
                var damage = shotEvent.Damage;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var unitEntity))
                {
                    return;
                }
                
                var unitPool = world.GetPool<Unit>();
                ref var unit = ref unitPool.Get(unitEntity);

                switch (shotType)
                {
                    case ShootType.Body:
                        damage -= unit.BodyArmor;
                        break;
                    case ShootType.Head:
                        damage -= unit.HeadArmor;
                        break;
                }
                
                unit.Health -= damage;
            }
        }
    }
}