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
            var currentItemCell = view.GetItemCell(_inventoryService.Value.CellsView.Cells);

            if (currentItemCell == null)
            {
                return;
            }
            
            if (armorCell.ChildItem != null)
            {
                PutItemToCell(armorCell.ChildItem, currentItemCell);
            }
            else
            {
                currentItemCell.ChildItem = null;
            }
                
            PutItemToArmorCell(view, armorCell);
        }

        private void PutItemToCell(ItemView itemView, CellView cell)
        {
            cell.ChildItem = itemView;
            itemView.SetParent(cell.Rect);
        }

        private void PutItemToArmorCell(ItemView itemView, ArmorCellView cell)
        {
            cell.ChildItem = itemView;
            itemView.SetParent(cell.Rect);
        }
    }
}