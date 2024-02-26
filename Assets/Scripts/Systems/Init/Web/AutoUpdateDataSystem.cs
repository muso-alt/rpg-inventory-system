using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class AutoUpdateDataSystem : IEcsRunSystem
    {
        private const float Interval = 10f;
        
        private readonly EcsWorldInject _eventsWorld = "events";
        private readonly EcsCustomInject<GameStateService> _gameStateService;
        
        private float _timer = 0f;
        
        public void Run(IEcsSystems systems)
        {
            _timer += Time.deltaTime;

            if (!_gameStateService.Value.IsPlaying || _timer < Interval)
            {
                return;
            }

            _eventsWorld.Value.SendEvent<UpdateDataEvent>();
            _timer = 0f;
        }
    }
}