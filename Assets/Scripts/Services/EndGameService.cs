using Inventory.Views;
using UnityEngine;

namespace Inventory.Services
{
    public class EndGameService : MonoBehaviour
    {
        [SerializeField] private EndGamePopupView _endGamePopup;

        public EndGamePopupView EndGamePopup => _endGamePopup;
    }
}