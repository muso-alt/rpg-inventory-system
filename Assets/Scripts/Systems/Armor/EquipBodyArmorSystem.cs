using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class EquipBodyArmorSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<Unit> _unitPool;
        private readonly EcsFilterInject<Inc<Player>> _playerFilter;
        private readonly EcsFilterInject<Inc<EquipBodyArmorEvent>> _equipEvent = "events";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _equipEvent.Value)
            {
                var pool = _equipEvent.Pools.Inc1;
                ref var equipEvent = ref pool.Get(entity);

                var view = equipEvent.View;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var itemEntity))
                {
                    continue;
                }

                var armorPool = world.GetPool<BodyArmor>();
                ref var armor = ref armorPool.Get(itemEntity);

                if (!_playerFilter.TryGetFirstEntityFromFilter(out var playerEntity))
                {
                    continue;
                }

                ref var unit = ref _unitPool.Value.Get(playerEntity);
                unit.BodyArmor = armor.Armor;
            }
        }
    }
}