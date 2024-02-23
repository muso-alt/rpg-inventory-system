using Inventory.Views;
using UnityEngine;

namespace Inventory.Services
{
    public class UnitsService : MonoBehaviour
    {
        [SerializeField] private UnitView _playerView;
        [SerializeField] private UnitView _enemyView;

        public UnitView PlayerView => _playerView;
        public UnitView EnemyView => _enemyView;
    }
}