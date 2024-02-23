using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class GunsView : MonoBehaviour
    {
        [SerializeField] private Button _shotButton;
        [SerializeField] private GunView _riffleView;
        [SerializeField] private GunView _pistolView;

        public Button ShotButton => _shotButton;
        public GunView RiffleView => _riffleView;
        public GunView PistolView => _pistolView;
    }
}