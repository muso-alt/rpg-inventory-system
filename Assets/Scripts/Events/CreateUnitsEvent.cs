using Inventory.Components;

namespace Inventory.Events
{
    public struct CreateUnitsEvent
    {
        public JsonUnit Player;
        public JsonUnit Enemy;
    }
}