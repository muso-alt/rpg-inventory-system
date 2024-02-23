using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class TargetTrackingSystem : IEcsRunSystem
    {
        private EcsPoolInject<Unit> _unitPool;
        private EcsFilterInject<Inc<Enemy>> _enemyFilter;
        private EcsFilterInject<Inc<Player>> _playerFilter;
        
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsFilterInject<Inc<PlayerShootEvent>> _playerShotFilter = "events";
        private readonly EcsFilterInject<Inc<EnemyShootEvent>> _enemyShotFilter = "events";

        public void Run(IEcsSystems systems)
        {
            UpdateEnemyHit();
            UpdatePlayerHit();
        }

        private void UpdateEnemyHit()
        {
            foreach (var entity in _playerShotFilter.Value)
            {
                var pool = _playerShotFilter.Pools.Inc1;
                ref var shotEvent = ref pool.Get(entity);

                if (!_enemyFilter.TryGetFirstEntityFromFilter(out var enemyEntity))
                {
                    return;
                }

                HitUnit(enemyEntity, shotEvent.Damage, shotEvent.ShootType);
            }
        }

        private void UpdatePlayerHit()
        {
            foreach (var entity in _enemyShotFilter.Value)
            {
                var pool = _enemyShotFilter.Pools.Inc1;
                ref var shotEvent = ref pool.Get(entity);

                if (!_playerFilter.TryGetFirstEntityFromFilter(out var playerEntity))
                {
                    return;
                }

                HitUnit(playerEntity, shotEvent.Damage, shotEvent.ShootType);
            }
        }

        private void HitUnit(int entity, int damage, ShootType type)
        {
            ref var unit = ref _unitPool.Value.Get(entity);
            var unitShootEvent = new BulletHitTargetEvent
                {View = unit.View, Damage = damage, ShootType = type};
                
            _eventWorld.Value.SendEvent(unitShootEvent);
        }
    }
}