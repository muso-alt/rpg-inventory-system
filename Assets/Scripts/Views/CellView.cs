using UnityEngine;

namespace Inventory.Views
{
    public class CellView : MonoBehaviour
    {
        public ItemView ChildItem { get; set; }
        
        [field: SerializeField] public RectTransform Rect { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Rect = GetComponent<RectTransform>();
        }
#endif
    }
}