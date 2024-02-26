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
    public class ContainerHandleSystem : IEcsRunSystem
    {
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
                
            _eventWorld.Value.SendEvent(createItemEvent);
            _eventWorld.Value.SendEvent(jsonData.Units);
        }
    }
}