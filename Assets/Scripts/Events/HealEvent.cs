using Inventory.Views;

namespace Inventory.Events
{
    public struct HealEvent
    {
        public UnitView ViewOfHealer;
        public ItemView ViewOfMedKit;
    }
}