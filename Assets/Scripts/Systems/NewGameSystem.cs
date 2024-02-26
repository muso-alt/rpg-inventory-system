using System.Linq;
using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class NewGameSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<ItemsData> _itemsData;
        private readonly EcsCustomInject<WebRequestView> _requestService;
        private readonly EcsCustomInject<GameStateService> _gameStateService;
        
        private readonly EcsFilterInject<Inc<NewGameEvent>> _newGameEvent = "events";
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _newGameEvent.Value)
            {
                var pool = _newGameEvent.Pools.Inc1;
                pool.Del(entity);
                
                SendDefaultEvent();
                SendUpdateEvent();
                
                _gameStateService.Value.IsPlaying = true;
            }
        }

        private void SendDefaultEvent()
        {
            var items = _itemsData.Value.ItemConfigs.Select((config, index) => new ItemData
                {Name = config.ItemName, CellIndex = index, CurrentCount = config.MaxStackSize}).ToList();

            var createItemEvent = new CreateItemEvent {Items = items};
            var player = new JsonUnit {Health = 100};
            var enemy = new JsonUnit {Health = 100};
            var createUnitsEvent = new CreateUnitsEvent {Player = player, Enemy = enemy};
                
            _eventWorld.Value.SendEvent(createItemEvent);
            _eventWorld.Value.SendEvent(createUnitsEvent);
        }

        private void SendUpdateEvent()
        {
            var initRemoteDataEvent = new UpdateDataEvent();
            _eventWorld.Value.SendEvent(initRemoteDataEvent);
        }
    }
}