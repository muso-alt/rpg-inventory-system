using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Systems
{
    public class AutoShootSystem : IEcsRunSystem
    {
        //Auto attack by enemy to player when player attacks
        private readonly EcsFilterInject<Inc<PlayerShootEvent>> _playerShootFilter = "events";
        
        private readonly EcsWorldInject _eventWorld = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerShootFilter.Value)
            {
                var shotType = (ShootType) Random.Range(0, 2);

                var enemyShootEvent = new EnemyShootEvent() {Damage = 15, ShootType = shotType};
                _eventWorld.Value.SendEvent(enemyShootEvent);
            }
        }
    }
}