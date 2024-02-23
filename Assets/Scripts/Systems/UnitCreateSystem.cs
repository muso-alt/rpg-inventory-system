using Inventory.Components;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class UnitCreateSystem : IEcsInitSystem
    {
        private EcsCustomInject<UnitsService> _unitsService;
        
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Init(IEcsSystems systems)
        {
            var playerEntity = CreateUnitEntity(_unitsService.Value.PlayerView);
            var playerPool = _defaultWorld.Value.GetPool<Player>();
            playerPool.Add(playerEntity);
            
            var enemyEntity = CreateUnitEntity(_unitsService.Value.EnemyView);
            var enemyPool = _defaultWorld.Value.GetPool<Enemy>();
            enemyPool.Add(enemyEntity);
        }

        private int CreateUnitEntity(UnitView view)
        {
            var entity = _defaultWorld.Value.NewEntity();

            var unitPool = _defaultWorld.Value.GetPool<Unit>();
            ref var unit = ref unitPool.Add(entity);

            view.EcsEventWorld = _eventWorld.Value;
            view.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(entity);

            unit.View = view;
            //TODO: FROM JSON INITIALIZE
            unit.Health = 100;

            return entity;
        }
    }
}