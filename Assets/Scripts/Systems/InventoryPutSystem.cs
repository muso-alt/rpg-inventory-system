using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.EventSystems;

namespace Inventory.Systems
{
    public class InventoryPutSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DragEvent>> _dragFilter = "events";
        private EcsCustomInject<InventoryService> _inventoryService;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dragFilter.Value)
            {
                var pool = _dragFilter.Pools.Inc1;
                ref var dragEvent = ref pool.Get(entity);
                
                var itemView = dragEvent.View;
                var dragType = dragEvent.Type;
                var eventData = dragEvent.EventData;

                if (dragType == DragType.End)
                {
                    TryPutItemToCell(itemView, eventData);
                }
            }
        }

        private void TryPutItemToCell(ItemView itemView, PointerEventData eventData)
        {
            var position = _inventoryService.Value.Camera.ScreenToWorldPoint(eventData.position);
            var cells = _inventoryService.Value.CellsView.Cells;

            foreach (var cell in cells)
            {
                if (!cell.Rect.IsInsideOtherRectByPosition(position))
                {
                    continue;
                }

                var currentItemCell = itemView.GetItemCell(cells);
                
                if (cell.ChildItem != null)
                {
                    PutItemToCell(cell.ChildItem, currentItemCell);
                }
                else
                {
                    currentItemCell.ChildItem = null;
                }
                
                PutItemToCell(itemView, cell);
                return;
            }
        }

        private void PutItemToCell(ItemView itemView, CellView cell)
        {
            cell.ChildItem = itemView;
            itemView.SetParent(cell.Rect);
        }
    }
}