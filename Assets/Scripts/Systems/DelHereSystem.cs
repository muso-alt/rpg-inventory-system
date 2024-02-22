using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class DelHereSystem<T> : IEcsRunSystem where T : struct
    {
        private readonly EcsFilterInject<Inc<T>> _delFilter = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _delFilter.Value)
            {
                _delFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}