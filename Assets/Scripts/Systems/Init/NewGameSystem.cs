using System.Collections.Generic;
using System.Linq;
using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json;
using UnityEngine;

namespace Inventory.Systems
{
    public class NewGameSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<ItemsData> _itemsData;
        private readonly EcsCustomInject<WebRequestView> _requestService;
        
        
        private readonly EcsFilterInject<Inc<NewGameEvent>> _newGameEvent = "events";
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _newGameEvent.Value)
            {
                var pool = _newGameEvent.Pools.Inc1;
                pool.Del(entity);
                
                SendCreateItems();
                SendUpdateEvent();
            }
        }

        private void SendCreateItems()
        {
            var items = _itemsData.Value.ItemConfigs.Select((config, index) => new ItemData
                {Name = config.ItemName, CellIndex = index, CurrentCount = config.MaxStackSize}).ToList();

            var createItemEvent = new CreateItemEvent {Items = items};
                
            _eventWorld.Value.SendEvent(createItemEvent);
            
            Debug.Log("End");
        }

        private void SendUpdateEvent()
        {
            var initRemoteDataEvent = new UpdateDataEvent();
            _eventWorld.Value.SendEvent(initRemoteDataEvent);
        }
    }
}