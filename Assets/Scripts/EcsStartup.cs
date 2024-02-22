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
        
        [SerializeField] private InventoryService _inventoryService;

        private void Start ()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new ItemsCreateSystem())
                .Add (new DragSystem())
                .Add(new InventoryPutSystem())
                .Add(new ItemInfoPopupSystem())
                .Add(new ItemQuantitySystem())
                .Add(new ItemCountDisplaySystem())
                
                .Add(new DelHereSystem<DragEvent>())
                .Add(new DelHereSystem<ClickEvent>())
                .Add(new DelHereSystem<ItemQuantityEvent>())
                .Add(new DelHereSystem<HealEvent>())
                
                .AddWorld (new EcsWorld (), "events")
                .Inject(_inventoryService, new ObjectsPool<ItemView>(_inventoryService.Data.View))
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