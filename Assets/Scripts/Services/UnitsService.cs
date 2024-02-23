using Inventory.Views;
using UnityEngine;

namespace Inventory.Services
{
    public class UnitsService : MonoBehaviour
    {
        [SerializeField] private UnitView _playerView;
        [SerializeField] private ArmorView _playerArmorView;
        
        [SerializeField] private UnitView _enemyView;

        public UnitView PlayerView => _playerView;
        public ArmorView PlayerArmorView => _playerArmorView;
        
        public UnitView EnemyView => _enemyView;
    }
}