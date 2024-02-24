using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class EndGameSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerDeadEvent>> _playerDeadFilter = "events";
        
        private readonly EcsCustomInject<EndGameService> _endGame;
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerDeadFilter.Value)
            {
                var pool = _playerDeadFilter.Pools.Inc1;
                ref var dieEvent = ref pool.Get(entity);

                var popup = _endGame.Value.EndGamePopup;
                popup.RestartButton.onClick.RemoveAllListeners();
                popup.RestartButton.onClick.AddListener(RestartGame);
                popup.Show();
            }
        }

        private void RestartGame()
        {
            _eventWorld.Value.SendEvent<RestartEvent>();
        }
    }
}