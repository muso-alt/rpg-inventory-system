using Inventory.Views;

namespace Inventory.Events
{
    public struct PlaceItemEvent
    {
        public ItemView View;
        public CellView Cell;
    }
}