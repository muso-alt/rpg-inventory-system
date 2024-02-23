using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class GunView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private int _bulletSpendCount;

        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; set; }
        public EcsWorld EcsEventWorld { get; set; }
        
        public Toggle Toggle => _toggle;
        public int BulletSpendCount => _bulletSpendCount;

        public void SetDamage(int count)
        {
            _damageText.text = count.ToString();
        }
    }
}