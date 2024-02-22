﻿using System;
using System.Globalization;
using Inventory.Components;
using Inventory.Data;
using Inventory.Events;
using Inventory.Services;
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

                switch (item.Type)
                {
                    case ItemType.Ammo:
                        ConfigureByAmmo(itemEntity);
                        break;
                    case ItemType.BodyArmor:
                        ConfigureByArmor(itemEntity);
                        break;
                    case ItemType.HeadArmor:
                        ConfigureByHeadArmor(itemEntity);
                        break;
                    case ItemType.MedKit:
                        ConfigureByMedKit(itemEntity);
                        break;
                }
                
                infoPopup.Show();
            }
        }

        private void ConfigureByAmmo(int entity)
        {
            ref var ammo = ref _defaultWorld.Value.GetCmpFromWorld<Ammo>(entity);
            
            SetInfoText($"Damage: {ammo.Damage}", "Buy");
            var ammoEvent = new AmmoEvent {Type = ammo.Type};
            SubscribeToButton(() => SendEvent(ammoEvent));
        }
        
        private void ConfigureByArmor(int entity)
        {
            ref var armor = ref _defaultWorld.Value.GetCmpFromWorld<BodyArmor>(entity);
            
            SetInfoText($"Armor: {armor.Armor}", "Equip");
            SubscribeToButton(() => SendEvent<HealEvent>());
        }
        
        private void ConfigureByHeadArmor(int entity)
        {
            ref var headArmor = ref _defaultWorld.Value.GetCmpFromWorld<HeadArmor>(entity);
            
            SetInfoText($"Armor: {headArmor.Armor}", "Equip");
            SubscribeToButton(() => SendEvent<HealEvent>());
        }
        
        private void ConfigureByMedKit(int entity)
        {
            ref var medKit = ref _defaultWorld.Value.GetCmpFromWorld<MedKit>(entity);
            
            SetInfoText($"Healing Power: {medKit.HealingPower}", "Heal");
            SubscribeToButton(() => SendEvent<HealEvent>());
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
            _defaultWorld.Value.SendEvent(data);
        }
    }
}