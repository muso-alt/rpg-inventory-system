using System;
using Cysharp.Threading.Tasks;
using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Inventory.Systems
{
    public class InitRemoteDataSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<WebRequestView> _requestView;
        private readonly EcsCustomInject<ItemsData> _itemsData;
        private readonly EcsWorldInject _eventWorld = "events";
        private readonly EcsFilterInject<Inc<InitRemoteDataEvent>> _dataEvent = "events";

        public void Init(IEcsSystems systems)
        {
            _requestView.Value.EcsEventWorld = _eventWorld.Value;
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dataEvent.Value)
            {
                var pool = _dataEvent.Pools.Inc1;
                pool.Del(entity);

                InitRemoteDataAsync().Forget();
            }
        }
        
        private async UniTask InitRemoteDataAsync()
        {
            
            var webRequest = new UnityWebRequest(_requestView.Value.URL + $"?id={_requestView.Value.InputId}", "GET")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            webRequest.SetRequestHeader("Content-Type", "multipart/form-data");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _requestView.Value.AccessToken);
            
            _requestView.Value.ToggleLoading(true);
            _requestView.Value.ToggleButtons(false);
            _requestView.Value.SetStatusText("Loading...");
            
            await webRequest.SendWebRequest();
            
            var jsonText = webRequest.downloadHandler.text;
            
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request failed: " + webRequest.error);
                _requestView.Value.SetStatusText("Error");
                return;
            }

            try
            {
                var containerEvent = JsonConvert.DeserializeObject<Container>(jsonText);
                
                _requestView.Value.CurrentAccountID = containerEvent.ID;
                _requestView.Value.SetId(containerEvent.ID);
                _eventWorld.Value.SendEvent(containerEvent);

                _requestView.Value.ToggleLoading(false);
                _requestView.Value.ToggleButtons(true);
                _requestView.Value.Hide();
            }
            catch (Exception e)
            {
                _requestView.Value.SetStatusText("Not found ID, try again");
                _requestView.Value.ToggleButtons(true);
                Debug.Log("Can't deserialize");
            }
        }
    }
}