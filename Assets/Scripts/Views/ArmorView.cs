using UnityEngine;

namespace Inventory.Views
{
    public class ArmorView : MonoBehaviour
    {
        [SerializeField] private ArmorCellView _headCell;
        [SerializeField] private ArmorCellView _bodyCell;

        public ArmorCellView HeadCell => _headCell;
        public ArmorCellView BodyCell => _bodyCell;
    }
}