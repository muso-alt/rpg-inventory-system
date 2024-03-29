﻿using Inventory.Components;
using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json;

namespace Inventory.Systems
{
    public class ContainerHandleSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<GameStateService> _gameStateService;
        private readonly EcsFilterInject<Inc<Container>> _containerFilter = "events";
        private readonly EcsWorldInject _eventWorld = "events";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _containerFilter.Value)
            {
                var pool = _containerFilter.Pools.Inc1;
                var container = pool.Get(entity);

                pool.Del(entity);

                ConfigureObjects(container);
            }
        }

        private void ConfigureObjects(Container container)
        {
            var jsonData = JsonConvert.DeserializeObject<JsonData>(container.Value);

            var createItemEvent = new CreateItemEvent {Items = jsonData.Items};
            var createUnitsEvent = new CreateUnitsEvent {Player = jsonData.Player, Enemy = jsonData.Enemy};
                
            _eventWorld.Value.SendEvent(createItemEvent);
            _eventWorld.Value.SendEvent(createUnitsEvent);
            
            _gameStateService.Value.IsPlaying = true;
        }
    }
}