using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class ArmorPlacementSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsCustomInject<UnitsService> _unitsService;
        private readonly EcsCustomInject<InventoryService> _inventoryService;
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

                SetItemToCell(view, _unitsService.Value.PlayerArmorView.BodyCell);
            }
        }

        private void UpdateHeadArmor()
        {
            foreach (var entity in _headArmorEquipEvent.Value)
            {
                var pool = _headArmorEquipEvent.Pools.Inc1;
                ref var equipEvent = ref pool.Get(entity);

                var view = equipEvent.View;

                SetItemToCell(view, _unitsService.Value.PlayerArmorView.HeadCell);
            }
        }

        private void SetItemToCell(ItemView view, ArmorCellView armorCell)
        {
            if (armorCell.ChildItem != null)
            {
                SwapItems(view);
            }
            
            view.SetParent(armorCell.Rect);
            armorCell.ChildItem = view;
        }

        private void SwapItems(ItemView view)
        {
            var currentViewCell = view.GetItemCell(_inventoryService.Value.CellsView.Cells);
            var headArmorCell = _unitsService.Value.PlayerArmorView.HeadCell;

            var oldItem = headArmorCell.ChildItem;
            
            oldItem.SetParent(currentViewCell.Rect);
            currentViewCell.ChildItem = oldItem;
        }
    }
}