using Inventory.Views;

namespace Inventory.Services
{
    public class ItemPlaceService
    {
        private readonly CellsView _cellsView;

        public ItemPlaceService(CellsView cellsView)
        {
            _cellsView = cellsView;
        }

        public void TryPutItemToCell(ItemView itemView, CellView cell)
        {
            var cells = _cellsView.Cells;
            
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