using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class PlayerArmorDamagingDisplaySystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<UnitsService> _unitsService;
        
        private readonly EcsPoolInject<Unit> _unitPool;
        private readonly EcsFilterInject<Inc<EnemyShootEvent>> _enemyShootFilter = "events";
        
        private EcsFilterInject<Inc<Player>> _playerFilter;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyShootFilter.Value)
            {
                var pool = _enemyShootFilter.Pools.Inc1;
                ref var shootEvent = ref pool.Get(entity);
                
                if (!_playerFilter.TryGetFirstEntityFromFilter(out var enemyEntity))
                {
                    return;
                }

                ref var unit = ref _unitPool.Value.Get(enemyEntity);
                
                var armorView = _unitsService.Value.PlayerArmorView;
                
                if (shootEvent.ShootType == ShootType.Body && unit.BodyArmor > 0)
                {
                    armorView.BodyCell.SetAnimation();
                }
                else if (shootEvent.ShootType == ShootType.Head && unit.HeadArmor > 0)
                {
                    armorView.HeadCell.SetAnimation();
                }
            }
        }
    }
}