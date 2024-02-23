using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Inventory.Systems;
using Inventory.Tools;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory 
{
    public sealed class EcsStartup : MonoBehaviour 
    {
        private EcsWorld _world;
        private IEcsSystems _systems;

        [SerializeField] private ItemsData _itemsData;
        [SerializeField] private InventoryService _inventoryService;
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private UnitsService _unitsService;

        private void Start ()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new ItemsCreateSystem())
                .Add(new ShootingSystem())
                .Add(new UnitCreateSystem())
                .Add(new UnitHealthDisplaySystem())
                .Add(new TargetTrackingSystem())
                .Add(new BulletHitSystem())
                .Add(new DragSystem())
                .Add(new InventoryPutSystem())
                .Add(new ItemInfoPopupSystem())
                .Add(new ItemQuantitySystem())
                .Add(new ItemCountDisplaySystem())
                .Add(new GunsInitSystem())
                .Add(new GunTriggerSystem())
                .Add(new GunsDamageDisplaySystem())
                
                .Add(new DelHereSystem<DragEvent>())
                .Add(new DelHereSystem<ClickEvent>())
                .Add(new DelHereSystem<HealEvent>())
                .Add(new DelHereSystem<ActiveGunEvent>())
                .Add(new DelHereSystem<GunTriggerEvent>())
                .Add(new DelHereSystem<EndItemEvent>())
                .Add(new DelHereSystem<ItemQuantityEvent>())
                .Add(new DelHereSystem<PlayerShootEvent>())
                .Add(new DelHereSystem<EnemyShootEvent>())
                .Add(new DelHereSystem<BulletHitTargetEvent>())
                
                .AddWorld (new EcsWorld (), "events")
                .Inject(_inventoryService, _itemsData, _cameraService,
                    _unitsService, new ObjectsPool<ItemView>(_itemsData.View))
                .Init ();
        }

        private void Update ()
        {
            _systems?.Run ();
        }

        private void OnDestroy () 
        {
            if (_systems != null) 
            {
                _systems.Destroy ();
                _systems = null;
            }

            if (_world != null) 
            {
                _world.Destroy ();
                _world = null;
            }
        }
    }
}