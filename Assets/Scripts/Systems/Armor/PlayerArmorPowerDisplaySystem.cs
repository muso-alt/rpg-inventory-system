using Inventory.Components;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class PlayerArmorPowerDisplaySystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<UnitsService> _unitsService;
        private readonly EcsPoolInject<Unit> _unitPool;
        private readonly EcsFilterInject<Inc<Player>> _playerFilter;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                if (!_unitPool.Value.Has(entity))
                {
                    continue;
                }
                
                ref var unit = ref _unitPool.Value.Get(entity);

                var armorView = _unitsService.Value.PlayerArmorView;
                
                armorView.BodyArmorPlace.SetArmor(unit.BodyArmor);
                armorView.HeadArmorPlace.SetArmor(unit.HeadArmor);
            }
        }
    }
}