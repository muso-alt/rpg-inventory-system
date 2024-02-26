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
        [SerializeField] private EndGameService _endGameService;
        [SerializeField] private WebRequestView _webRequestView;

        private void Start ()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new NewGameSystem())
                .Add(new LoginSystem())
                .Add(new InitRemoteDataSystem())
                .Add(new ContainerHandleSystem())
                
                .Add(new ItemsCreateSystem())
                .Add(new UnitCreateSystem())
                
                .Add(new ShootingSystem())
                .Add(new AutoShootSystem())
                .Add(new TargetTrackingSystem())
                .Add(new DragSystem())
                .Add(new DragEndHandleSystem())
                .Add(new ItemInfoPopupSystem())
                .Add(new PlayerHealSystem())
                .Add(new ItemQuantitySystem())
                .Add(new GunsInitSystem())
                .Add(new GunTriggerSystem())
                .Add(new PlayerArmorAutoSetSystem())
                .Add(new ArmorPlacementSystem())
                .Add(new EquipBodyArmorSystem())
                .Add(new EquipHeadArmorSystem())
                .Add(new UnitDieSystem())
                .Add(new DeadUnitHandleSystem())
                .Add(new BonusItemSystem())
                .Add(new EndGameSystem())
                
                //Without send events
                .Add(new AutoUpdateDataSystem())
                .Add(new BulletHitSystem())
                .Add(new UnitHealSystem())
                .Add(new ItemDeleteSystem())
                .Add(new ItemPlaceSystem())
                .Add(new RaiseEnemySystem())
                .Add(new RestartSceneSystem())
                .Add(new UpdateRemoteDataSystem())
                
                //Displays systems
                .Add(new ItemCountDisplaySystem())
                .Add(new GunsDamageDisplaySystem())
                .Add(new UnitHealthDisplaySystem())
                .Add(new PlayerArmorPowerDisplaySystem())
                .Add(new PlayerArmorDamagingDisplaySystem())
                
                .Add(new DelHereSystem<DragEvent>())
                .Add(new DelHereSystem<ClickEvent>())
                .Add(new DelHereSystem<HealEvent>())
                .Add(new DelHereSystem<ActiveGunEvent>())
                .Add(new DelHereSystem<GunTriggerEvent>())
                .Add(new DelHereSystem<DeleteItemEvent>())
                .Add(new DelHereSystem<ItemQuantityEvent>())
                .Add(new DelHereSystem<PlayerShootEvent>())
                .Add(new DelHereSystem<EnemyShootEvent>())
                .Add(new DelHereSystem<BulletHitTargetEvent>())
                .Add(new DelHereSystem<EquipHeadArmorEvent>())
                .Add(new DelHereSystem<PlayerHealEvent>())
                .Add(new DelHereSystem<EquipBodyArmorEvent>())
                .Add(new DelHereSystem<UnitDieEvent>())
                .Add(new DelHereSystem<PlayerDeadEvent>())
                .Add(new DelHereSystem<EnemyDeadEvent>())
                .Add(new DelHereSystem<CreateUnitsEvent>())
                
                .AddWorld (new EcsWorld(), "events")
                
                .Inject(_inventoryService, _itemsData, _cameraService, 
                    _unitsService, new ObjectsPool<ItemView>(_itemsData.View), 
                    new DeletedItemsPool(), _endGameService, _webRequestView, 
                    new GameStateService(), new ItemPlaceService(_inventoryService.CellsView))
                
                .Init ();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy() 
        {
            if (_systems != null) 
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null) 
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}