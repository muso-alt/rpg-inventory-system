using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.EventSystems;

namespace Inventory.Systems
{
    public class DragEndHandleSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DragEvent>> _dragFilter = "events";
        private EcsCustomInject<InventoryService> _inventoryService;
        private EcsCustomInject<CameraService> _cameraService;
        private readonly EcsCustomInject<ItemPlaceService> _placeService;
        
        private readonly EcsWorldInject _eventsWorld = "events";
        
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
            var position = _cameraService.Value.Camera.ScreenToWorldPoint(eventData.position);
            var cells = _inventoryService.Value.CellsView.Cells;

            foreach (var cell in cells)
            {
                if (!cell.Rect.IsInsideOtherRectByPosition(position))
                {
                    continue;
                }
                
                _placeService.Value.TryPutItemToCell(itemView, cell);
                break;
            }
        }
    }
}