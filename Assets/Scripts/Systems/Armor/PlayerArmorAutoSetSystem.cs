using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class PlayerArmorAutoSetSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<InventoryService> _inventoryService;
        private readonly EcsFilterInject<Inc<CreateUnitsEvent>> _unitsFilter = "events";
        
        private readonly EcsPoolInject<Item> _itemsPool;
        private readonly EcsFilterInject<Inc<Item>> _itemFilter;

        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitsFilter.Value)
            {
                var pool = _unitsFilter.Pools.Inc1;
                var unitEvent = pool.Get(entity);

                var bodyArmor = GetViewByName(unitEvent.Player.EquipBodyArmorName);
                var headArmor = GetViewByName(unitEvent.Player.EquipHeadArmorName);

                if (bodyArmor != null)
                {
                    var equipBodyArmorEvent = new EquipBodyArmorEvent {View = bodyArmor};
                    _eventWorld.Value.SendEvent(equipBodyArmorEvent);
                }

                if (headArmor != null)
                {
                    var equipHeadArmorEvent = new EquipHeadArmorEvent() {View = headArmor};
                    _eventWorld.Value.SendEvent(equipHeadArmorEvent);
                }
            }
        }

        private ItemView GetViewByName(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return null;
            }
            
            foreach (var entity in _itemFilter.Value)
            {
                var item = _itemsPool.Value.Get(entity);
                
                if (item.Name.Equals(itemName))
                {
                    return item.View;
                }
            }

            return null;
        }
    }
}