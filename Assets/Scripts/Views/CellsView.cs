using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Views
{
    public class CellsView : MonoBehaviour
    {
        [SerializeField] private List<CellView> _cells;
        public List<CellView> Cells => _cells;
    }
}