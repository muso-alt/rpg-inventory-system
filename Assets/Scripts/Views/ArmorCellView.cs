using TMPro;
using UnityEngine;

namespace Inventory.Views
{
    public class ArmorCellView : MonoBehaviour
    {
        public ItemView ChildItem { get; set; }
        
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_Text _armorValueText;

        public RectTransform Rect => _rect;

        public void SetArmor(int armorValue)
        {
            _armorValueText.text = armorValue.ToString();
        }
    }
}