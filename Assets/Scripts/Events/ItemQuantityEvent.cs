using Inventory.Views;

namespace Inventory.Events
{
    public struct ItemQuantityEvent
    {
        public ItemView View;
        public int Quantity;
    }
}