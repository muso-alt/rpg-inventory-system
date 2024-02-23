using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;

        public Button RestartButton => _restartButton;
    }
}