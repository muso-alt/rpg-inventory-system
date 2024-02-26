using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class ItemPlaceSystem : IEcsRunSystem
    {
        //private readonly EcsFilterInject<Inc<PlaceItemEvent>> _placeItemFilter = "events";
        private readonly EcsCustomInject<InventoryService> _inventoryService;

        public void Run(IEcsSystems systems)
        {
            /*foreach (var entity in _placeItemFilter.Value)
            {
                //var pool = _placeItemFilter.Pools.Inc1;
                //var placeItemEvent = pool.Get(entity);
                
               // TryPutItemToCell(placeItemEvent.View, placeItemEvent.Cell);
            }*/
        }
        
        private void TryPutItemToCell(ItemView itemView, CellView cell)
        {
            var cells = _inventoryService.Value.CellsView.Cells;
            
            var currentItemCell = itemView.GetItemCell(cells);

            if (cell.ChildItem != null)
            {
                PutItemToCell(cell.ChildItem, currentItemCell);
            }
            else if(currentItemCell != null)
            {
                currentItemCell.ChildItem = null;
            }
                
            PutItemToCell(itemView, cell);
        }

        private void PutItemToCell(ItemView itemView, CellView cell)
        {
            cell.ChildItem = itemView;
            itemView.SetParent(cell.Rect);
        }
    }
}