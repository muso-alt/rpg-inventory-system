using Inventory.Views;
using UnityEngine.EventSystems;

namespace Inventory.Events
{
    public struct DragEvent
    {
        public DragType Type;
        public ItemView View;
        public PointerEventData EventData;
    }

    public enum DragType
    {
        Begin,
        Drag,
        End
    }
}