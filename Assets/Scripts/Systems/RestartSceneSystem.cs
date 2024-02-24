using Inventory.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.SceneManagement;

namespace Inventory.Systems
{
    public class RestartSceneSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RestartEvent>> _restartEvent = "events";
        
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _restartEvent.Value)
            {
                var pool = _restartEvent.Pools.Inc1;
                pool.Del(entity);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}