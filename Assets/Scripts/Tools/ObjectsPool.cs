using System.Collections.Generic;
using Inventory.Views;
using UnityEngine;

namespace Inventory.Tools
{
    public class ObjectsPool<T> where T : MonoBehaviour
    {
        private readonly T _item;
        private readonly List<T> _pool = new List<T>();

        public ObjectsPool(T spawnItem)
        {
            _item = spawnItem;
        }

        public T GetItem(Transform parent = null)
        {
            if (_pool.Count <= 0)
            {
                return Object.Instantiate(_item, parent);
            }
            
            var view = _pool[0];
            _pool.Remove(view);
            view.gameObject.SetActive(true);
            return view;
        }

        public void Return(T view)
        {
            view.gameObject.SetActive(false);
            _pool.Add(view);
        }
    }
}