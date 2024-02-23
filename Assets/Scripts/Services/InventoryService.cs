using Inventory.Views;
using UnityEngine;

namespace Inventory.Services
{
    public class InventoryService : MonoBehaviour
    {
        [SerializeField] private RectTransform _mainContent;
        [SerializeField] private CellsView _cellsView;
        [SerializeField] private InfoView _infoView;
        [SerializeField] private GunsView _gunsView;

        public RectTransform MainContent => _mainContent;
        public CellsView CellsView => _cellsView;
        public InfoView InfoPopup => _infoView;
        public GunsView Guns => _gunsView;
    }
}