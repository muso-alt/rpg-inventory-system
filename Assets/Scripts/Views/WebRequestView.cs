using System;
using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Services
{
    public class WebRequestView : MonoBehaviour
    {
        [SerializeField] private string _url;
        [SerializeField] private string _accessToken;

        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private TMP_InputField _inputField;
        
        [SerializeField] private GameObject _loadingObject;

        private Container _container;
        
        public int CurrentAccountID { get; set; }
        public int InputId { get; private set; }
        public EcsWorld EcsEventWorld { get; set; }

        public string URL => _url;

        public string AccessToken => _accessToken;

        private void Start()
        {
            _inputField.onValueChanged.AddListener(OnInputValueChange);
            
            _loginButton.onClick.AddListener(SendLoginEvent);
            _newGameButton.onClick.AddListener(SendNewGameEvent);
        }

        private void SendNewGameEvent()
        {
            EcsEventWorld.SendEvent(new NewGameEvent());
        }
        
        private void SendLoginEvent()
        {
            var loginEvent = new LoginEvent {ID = InputId};
            EcsEventWorld.SendEvent(loginEvent);
        }

        private void OnInputValueChange(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            
            if (int.TryParse(input, out var intValue))
            {
                InputId = intValue;
            }
        }

        public void ToggleLoading(bool toggleValue)
        {
            _loginButton.interactable = !toggleValue;
            _newGameButton.interactable = !toggleValue;
            _loadingObject.SetActive(toggleValue);
        }

        private void OnDestroy()
        {
            _loginButton.onClick.RemoveAllListeners();
            _newGameButton.onClick.RemoveAllListeners();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}