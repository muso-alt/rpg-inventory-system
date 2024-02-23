using Inventory.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class UnitHealthDisplaySystem : IEcsRunSystem
    {
        private EcsPoolInject<Unit> _unitsPool;
        private EcsFilterInject<Inc<Unit>> _unitFilter;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitFilter.Value)
            {
                ref var unit = ref _unitsPool.Value.Get(entity);

                unit.View.SetHealth(unit.Health);
            }
        }
    }
}