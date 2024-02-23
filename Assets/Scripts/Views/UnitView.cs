using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Slider _healthBar;

        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; set; }
        
        public void SetHealth(int count)
        {
            _healthBar.value = count;
            _healthText.text = count.ToString();
        }
    }
}