using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Services
{
    public class ServerRequestService : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ids;
        [SerializeField] private Button _loginButton;
        
        [SerializeField] private GameObject _loadingObject;

        public Button LoginButton => _loginButton;

        public void SetIds(string ids)
        {
            _ids.text = ids;
        }

        public void ToggleLoading(bool toggleValue)
        {
            _loadingObject.SetActive(toggleValue);
        }
    }
}