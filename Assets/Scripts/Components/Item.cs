using Inventory.Data;
using Inventory.Views;
using UnityEngine;

namespace Inventory.Components
{
    public struct Item
    {
        public string Name;
        public ItemType Type;
        public ItemView View;
        public GameObject Icon;
        
        public int MaxStackSize;
        public int CurrentCount;
        
        public float Weight;
    }
}