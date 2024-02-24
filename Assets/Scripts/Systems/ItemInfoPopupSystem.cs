using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Inventory.Systems
{
    public class ItemInfoPopupSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ClickEvent>> _clickFilter = "events";
        private EcsCustomInject<InventoryService> _service;
        
        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsWorldInject _eventWorld = "events";
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _clickFilter.Value)
            {
                var pool = _clickFilter.Pools.Inc1;
                ref var clickEvent = ref pool.Get(entity);

                var view = clickEvent.View;

                if (!view.PackedEntityWithWorld.Unpack(out var world, out var itemEntity))
                {
                    continue;
                }

                var itemPool = world.GetPool<Item>();
                ref var item = ref itemPool.Get(itemEntity);
                
                var infoPopup = _service.Value.InfoPopup;
                
                infoPopup.SetItemText(item.Name);
                infoPopup.SetWeightText($"Weight: {item.Weight}");
                infoPopup.Icon = Object.Instantiate(item.Icon, infoPopup.Content);
                
                SubscribeToCloseButton();
                SubscribeToDeleteButton(item.View);

                switch (item.Type)
                {
                    case ItemType.Ammo:
                        ConfigureByAmmo(ref item);
                        break;
                    case ItemType.BodyArmor:
                        ConfigureByBodyArmor(ref item);
                        break;
                    case ItemType.HeadArmor:
                        ConfigureByHeadArmor(ref item);
                        break;
                    case ItemType.MedKit:
                        ConfigureByMedKit(ref item);
                        break;
                }
                
                infoPopup.Show();
            }
        }

        private void SubscribeToDeleteButton(ItemView view)
        {
            var deleteEvent = new DeleteItemEvent {View = view};
            
            _service.Value.InfoPopup.DeleteButton.onClick.RemoveAllListeners();
            _service.Value.InfoPopup.DeleteButton.onClick.AddListener(() => SendEvent(deleteEvent));
        }

        private void ConfigureByAmmo(ref Item item)
        {
            var entity = item.View.PackedEntityWithWorld.Id;
            ref var ammo = ref _defaultWorld.Value.GetCmpFromWorld<Ammo>(entity);
            
            SetInfoText($"Damage: {ammo.Damage}", "Buy");
            var quantity = new ItemQuantityEvent {View = item.View, Quantity = item.MaxStackSize};
            SubscribeToButton(() => SendEvent(quantity));
        }
        
        private void ConfigureByBodyArmor(ref Item item)
        {
            var entity = item.View.PackedEntityWithWorld.Id;
            ref var armor = ref _defaultWorld.Value.GetCmpFromWorld<BodyArmor>(entity);
            
            SetInfoText($"Armor: {armor.Armor}", "Equip");
            var armorEvent = new EquipBodyArmorEvent {View = item.View};
            SubscribeToButton(() => SendEvent(armorEvent));
        }
        
        private void ConfigureByHeadArmor(ref Item item)
        {
            var entity = item.View.PackedEntityWithWorld.Id;
            ref var headArmor = ref _defaultWorld.Value.GetCmpFromWorld<HeadArmor>(entity);
            
            SetInfoText($"Armor: {headArmor.Armor}", "Equip");
            var armorEvent = new EquipHeadArmorEvent {View = item.View};
            SubscribeToButton(() => SendEvent(armorEvent));
        }
        
        private void ConfigureByMedKit(ref Item item)
        {
            var entity = item.View.PackedEntityWithWorld.Id;
            ref var medKit = ref _defaultWorld.Value.GetCmpFromWorld<MedKit>(entity);
            
            SetInfoText($"Healing Power: {medKit.HealingPower}", "Heal");
            var playerHealEvent = new PlayerHealEvent {View = item.View};
            SubscribeToButton(() => SendEvent(playerHealEvent));
        }

        private void SetInfoText(string performanceText, string activeButtonText)
        {
            _service.Value.InfoPopup.SetPerformanceText(performanceText);
            _service.Value.InfoPopup.SetActiveButtonText(activeButtonText);
        }

        private void SubscribeToButton(UnityAction action)
        {
            var activeButton = _service.Value.InfoPopup.ActiveButton;
            
            activeButton.onClick.RemoveAllListeners();
            activeButton.onClick.AddListener(action);
        }

        private void SubscribeToCloseButton()
        {
            var infoPopup = _service.Value.InfoPopup;
            infoPopup.CloseButton.onClick.RemoveAllListeners();
            infoPopup.CloseButton.onClick.AddListener(HidePopup);
        }

        private void HidePopup()
        {
            _service.Value.InfoPopup.Hide();
            Object.Destroy(_service.Value.InfoPopup.Icon);
        }

        private void SendEvent<T>(T data = default) where T : struct
        {
           HidePopup();
            _eventWorld.Value.SendEvent(data);
        }
    }
}