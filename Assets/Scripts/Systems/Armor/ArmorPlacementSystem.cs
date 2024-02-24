using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class ArmorPlacementSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<UnitsService> _unitsService;
        private readonly EcsCustomInject<InventoryService> _inventoryService;
        
        private readonly EcsWorldInject _eventWorld = "events";
        
        private readonly EcsFilterInject<Inc<EquipHeadArmorEvent>> _headArmorEquipEvent = "events";
        private readonly EcsFilterInject<Inc<EquipBodyArmorEvent>> _bodyArmorEquipEvent = "events";
        
        public void Run(IEcsSystems systems)
        {
            UpdateBodyArmor();
            UpdateHeadArmor();
        }

        private void UpdateBodyArmor()
        {
            foreach (var entity in _bodyArmorEquipEvent.Value)
            {
                var pool = _bodyArmorEquipEvent.Pools.Inc1;
                ref var equipEvent = ref pool.Get(entity);

                var view = equipEvent.View;

                SendEvent(view, _unitsService.Value.PlayerArmorView.BodyCell);
            }
        }

        private void UpdateHeadArmor()
        {
            foreach (var entity in _headArmorEquipEvent.Value)
            {
                var pool = _headArmorEquipEvent.Pools.Inc1;
                ref var equipEvent = ref pool.Get(entity);

                var view = equipEvent.View;

                SendEvent(view, _unitsService.Value.PlayerArmorView.HeadCell);
            }
        }

        private void SendEvent(ItemView view, CellView cell)
        {
            var itemPlaceEvent = new PlaceItemEvent
                {View = view, Cell = cell};
            
            _eventWorld.Value.SendEvent(itemPlaceEvent);
        }
    }
}