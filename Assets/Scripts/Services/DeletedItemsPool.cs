using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Services
{
    public class DeletedItemsPool
    {
        private readonly List<int> _items = new List<int>();

        public void AddToDeleted(int entity)
        {
            _items.Add(entity);
        }

        public bool TryGetRandomItem(out int entity)
        {
            entity = _items[Random.Range(0, _items.Count)];

            if (_items.Count <= 0)
            {
                return false;
            }
            
            _items.Remove(entity);
            return true;
        }
    }
}