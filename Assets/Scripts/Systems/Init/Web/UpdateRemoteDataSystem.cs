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
        private readonly EcsCustomInject<UnitsService> _unitsService;
        private readonly EcsCustomInject<ItemsData> _itemsData;
        
        private EcsPoolInject<Item> _itemsPool;
        private EcsPoolInject<Unit> _unitsPool;
        private EcsFilterInject<Inc<Item>> _itemFilter;
        
        private EcsFilterInject<Inc<Player>> _playerFilter;
        private EcsFilterInject<Inc<Enemy>> _enemyFilter;
        
        private readonly EcsFilterInject<Inc<UpdateDataEvent>> _dataEvent = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dataEvent.Value)
            {
                var pool = _dataEvent.Pools.Inc1;

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
                    CellIndex = GetCellIndex(itemCell),
                };
                
                items.Add(jsonItem);
            }

            var bodyArmorCell = _unitsService.Value.PlayerArmorView.BodyCell;
            var headArmorCell = _unitsService.Value.PlayerArmorView.HeadCell;
            
            jsonData.Items = items;
            
            jsonData.Player = GetUnitsData(_playerFilter);
            jsonData.Player.EquipBodyArmorName = GetPlayerEquipArmorName(bodyArmorCell);
            jsonData.Player.EquipHeadArmorName = GetPlayerEquipArmorName(headArmorCell);

            jsonData.Enemy = GetUnitsData(_enemyFilter);
            //May set enemy armors

            var data = JsonConvert.SerializeObject(jsonData);
            var id = _requestView.Value.CurrentAccountID;
            var container = new Container {Value = data, ID = id};
            return container;
        }

        private JsonUnit GetUnitsData<T>(EcsFilterInject<Inc<T>> filter) where T : struct
        {
            var unitData = new JsonUnit();
            
            if (filter.TryGetFirstEntityFromFilter(out var entity))
            {
                var unit = _unitsPool.Value.Get(entity);
                unitData.Health = unit.Health;
            }
            
            return unitData;
        }

        private string GetPlayerEquipArmorName(CellView cell)
        {
            var armor = cell.ChildItem;

            if (armor == null)
            {
                return string.Empty;
            }

            var item = _itemsPool.Value.Get(armor.PackedEntityWithWorld.Id);
            return item.Name;
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

            return -1;
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
            _requestView.Value.SetId(response.ID);
        }
    }
}