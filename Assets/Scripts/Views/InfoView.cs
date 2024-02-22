using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class InfoView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _performance;
        [SerializeField] private TMP_Text _weight;
        [SerializeField] private TMP_Text _activeButtonText;

        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _activeButton;
        [SerializeField] private Button _deleteButton;

        public Button CloseButton => _closeButton;
        public Button ActiveButton => _activeButton;
        public Button DeleteButton => _deleteButton;

        public RectTransform Content => _content;
        public GameObject Icon { get; set; }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void SetItemText(string itemName)
        {
            _itemName.text = itemName;
        }
        
        public void SetPerformanceText(string performanceText)
        {
            _performance.text = performanceText;
        }
        
        public void SetWeightText(string weight)
        {
            _weight.text = weight;
        }
        
        public void SetActiveButtonText(string activeButtonText)
        {
            _activeButtonText.text = activeButtonText;
        }
    }
}