using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Services
{
    public static class EcsLiteExtensions
    {
        public static void SendEvent<T>(this EcsWorld world, T data = default) where T : struct
        {
            var entity = world.NewEntity();
            ref var eventCmp = ref world.GetPool<T>().Add(entity);
            eventCmp = data;
        }

        public static ref T GetByCreate<T>(this EcsWorld world, int entity) where T : struct
        {
            var cmpPool = world.GetPool<T>();
            return ref cmpPool.Add(entity);
        }

        public static ref T GetCmpFromWorld<T>(this EcsWorld world, int entity) where T : struct
        {
            var medKitPool = world.GetPool<T>();
            return ref medKitPool.Get(entity);
        }
        
        public static bool TryGetFirstEntityFromFilter<T>(this EcsFilterInject<Inc<T>> filter,
            out int id) where T : struct
        {
            id = 0;
            
            foreach (var entity in filter.Value)
            {
                id = entity;
                return true;
            }

            return false;
        }
    }
}