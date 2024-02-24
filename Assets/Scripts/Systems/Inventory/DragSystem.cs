using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.Systems
{
    public class DragSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DragEvent>> _dragFilter = "events";
        
        private EcsCustomInject<InventoryService> _inventoryService;
        private EcsCustomInject<CameraService> _cameraService;
        
        private GameObject _activeDragItem;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dragFilter.Value)
            {
                var pool = _dragFilter.Pools.Inc1;
                ref var dragEvent = ref pool.Get(entity);
                
                var itemView = dragEvent.View;
                var dragType = dragEvent.Type;
                var eventData = dragEvent.EventData;

                if (!itemView.PackedEntityWithWorld.Unpack(out var world, out var itemEntity))
                {
                    continue;
                }
                
                var itemPool = world.GetPool<Item>();
                ref var item = ref itemPool.Get(itemEntity);

                switch (dragType)
                {
                    case DragType.Begin:
                        Begin(ref item, eventData);
                        break;
                    case DragType.Drag:
                        Drag(ref item, eventData);
                        break;
                    case DragType.End:
                        End(ref item, eventData);
                        break;
                }
            }
        }    

        private void Begin(ref Item item, PointerEventData eventData)
        {
            _activeDragItem = Object.Instantiate(item.Icon, _inventoryService.Value.MainContent);
        }

        private void Drag(ref Item item, PointerEventData eventData)
        {
            var newPosition = _cameraService.Value.Camera.ScreenToWorldPoint(eventData.position);
            newPosition.z = 0;
            _activeDragItem.transform.position = newPosition;
        }
        
        private void End(ref Item item, PointerEventData eventData)
        {
            Object.Destroy(_activeDragItem);
            _activeDragItem = null;
        }
    }
}