using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Inventory.Systems
{
    public class UpdateRemoteDataSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<WebRequestView> _requestView;
        private readonly EcsCustomInject<InventoryService> _inventoryService;
        private readonly EcsCustomInject<ItemsData> _itemsData;
        
        private EcsPoolInject<Item> _itemsPool;
        private EcsFilterInject<Inc<Item>> _itemFilter;
        
        private readonly EcsFilterInject<Inc<UpdateDataEvent>> _dataEvent = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dataEvent.Value)
            {
                var pool = _dataEvent.Pools.Inc1;

                var id = _requestView.Value.CurrentAccountID;
                var container = GetContainer();
                
                pool.Del(entity);

                PostRequestAsync(container).Forget();
            }
        }
        
        private Container GetContainer()
        {
            var jsonData = new JsonData();
            var items = new List<ItemData>();

            foreach (var entity in _itemFilter.Value)
            {
                ref var item = ref _itemsPool.Value.Get(entity);

                var itemCell = item.View.GetItemCell(_inventoryService.Value.CellsView.Cells);

                var jsonItem = new ItemData
                {
                    Name = item.Name,
                    CurrentCount = item.CurrentCount,
                    CellIndex = GetCellIndex(itemCell)
                };
                
                items.Add(jsonItem);
            }

            jsonData.Items = items;
            
            jsonData.Units.PlayerHealth = 100;
            jsonData.Units.EnemyHealth = 100;

            var data = JsonConvert.SerializeObject(jsonData);
            var container = new Container {Value = data, ID = 7};
            return container;
        }

        private int GetCellIndex(CellView view)
        {
            for (var i = 0; i < _inventoryService.Value.CellsView.Cells.Count; i++)
            {
                var cell = _inventoryService.Value.CellsView.Cells[i];
                
                if (cell == view)
                {
                    return i;
                }
            }

            return 0;
        }
        
        private async UniTask PostRequestAsync(Container container)
        {
            var form = new WWWForm();
            form.AddField("value", container.Value);

            var url = _requestView.Value.URL;

            if (container.ID != 0)
            {
                url += $"?id={container.ID}";
            }

            var webRequest = UnityWebRequest.Post(url, form);
        
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Authorization", "Bearer " + _requestView.Value.AccessToken);

            _requestView.Value.ToggleLoading(true);
            await webRequest.SendWebRequest();
            _requestView.Value.ToggleLoading(false);
            _requestView.Value.Hide();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request failed: " + webRequest.error);
                return;
            }

            Debug.Log("Response: " + webRequest.downloadHandler.text);

            var response = JsonConvert.DeserializeObject<Container>(webRequest.downloadHandler.text);
            _requestView.Value.CurrentAccountID = response.ID;
        }
    }
}