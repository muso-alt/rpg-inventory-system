using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class UnitCreateSystem : IEcsRunSystem
    {
        private EcsCustomInject<UnitsService> _unitsService;
        
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsFilterInject<Inc<CreateUnitsEvent>> _unitsFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitsFilter.Value)
            {
                var pool = _unitsFilter.Pools.Inc1;
                var unitEvent = pool.Get(entity);
                
                CreatePlayer(unitEvent.Player);
                CreateEnemy(unitEvent.Enemy);
            }
        }

        private void CreatePlayer(JsonUnit unit)
        {
            var playerEntity = CreateUnitEntity(_unitsService.Value.PlayerView, unit.Health);
            var playerPool = _defaultWorld.Value.GetPool<Player>();
            playerPool.Add(playerEntity);
        }

        private void CreateEnemy(JsonUnit unit)
        {
            var enemyEntity = CreateUnitEntity(_unitsService.Value.EnemyView, unit.Health);
            var enemyPool = _defaultWorld.Value.GetPool<Enemy>();
            enemyPool.Add(enemyEntity);
        }

        private int CreateUnitEntity(UnitView view, int unitHealth)
        {
            var entity = _defaultWorld.Value.NewEntity();

            var unitPool = _defaultWorld.Value.GetPool<Unit>();
            ref var unit = ref unitPool.Add(entity);

            view.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(entity);

            unit.View = view;
            unit.Health = unitHealth;

            return entity;
        }
    }
}