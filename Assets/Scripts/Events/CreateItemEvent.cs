using System.Collections.Generic;
using Inventory.Components;
using Inventory.Data;

namespace Inventory.Events
{
    public struct CreateItemEvent
    {
        public List<ItemData> Items;
    }
}