using UnityEngine;

namespace Inventory.Views
{
    public class ArmorView : MonoBehaviour
    {
        [SerializeField] private ArmorPlaceView _headArmorPlace;
        [SerializeField] private CellView _headCell;
        
        [SerializeField] private ArmorPlaceView _bodyArmorPlace;
        [SerializeField] private CellView _bodyCell;

        public ArmorPlaceView HeadArmorPlace => _headArmorPlace;
        public ArmorPlaceView BodyArmorPlace => _bodyArmorPlace;
        
        public CellView HeadCell => _headCell;
        public CellView BodyCell => _bodyCell;
    }
}