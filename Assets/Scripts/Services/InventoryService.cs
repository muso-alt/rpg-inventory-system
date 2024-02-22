using Inventory.Data;
using Inventory.Views;
using UnityEngine;

namespace Inventory.Services
{
    public class InventoryService : MonoBehaviour
    {
        [SerializeField] private ItemsData _itemsData;
        [SerializeField] private RectTransform _mainContent;
        [SerializeField] private Camera _mainCamera;
        
        [SerializeField] private CellsView _cellsView;
        [SerializeField] private InfoView _infoView;

        public ItemsData Data => _itemsData;
        public RectTransform MainContent => _mainContent;
        public Camera Camera => _mainCamera;
        public CellsView CellsView => _cellsView;
        public InfoView InfoPopup => _infoView;
    }
}