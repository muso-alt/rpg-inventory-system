using Inventory.Views;
using UnityEngine.EventSystems;

namespace Inventory.Events
{
    public struct ClickEvent
    {
        public ItemView View;
        public PointerEventData EventData;
    }
}