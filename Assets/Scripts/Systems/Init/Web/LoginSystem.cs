using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class LoginSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<LoginEvent>> _loginEvent = "events";
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _loginEvent.Value)
            {
                var pool = _loginEvent.Pools.Inc1;
                ref var loginEvent = ref pool.Get(entity);
                
                //Here is must be logic
                var id = loginEvent.ID;
                
                pool.Del(entity);

                var initRemoteDataEvent = new InitRemoteDataEvent {ID = id};
                _eventWorld.Value.SendEvent(initRemoteDataEvent);
            }
        }
    }
}