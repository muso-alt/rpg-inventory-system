using Inventory.Events;
using Inventory.Services;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.Views
{
    public class ItemView : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private RectTransform _content;

        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; set; }
        public EcsWorld EcsEventWorld { get; set; }

        public void SetCountText(int count)
        {
            _countText.text = count.ToString();
        }

        public void ToggleCountText(bool toggleValue)
        {
            _countText.gameObject.SetActive(toggleValue);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SendDragEvent(eventData, DragType.Begin);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SendDragEvent(eventData, DragType.Drag);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SendDragEvent(eventData, DragType.End);
        }

        private void SendDragEvent(PointerEventData eventData, DragType type)
        {
            var eventCmp = new DragEvent {View = this, Type = type, EventData = eventData};
            EcsEventWorld.SendEvent(eventCmp);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var eventCmp = new ClickEvent {View = this, EventData = eventData};
            EcsEventWorld.SendEvent(eventCmp);
        }

        public void SetParent(RectTransform parent)
        {
            var item = transform;
            
            item.SetParent(parent);
            item.localPosition = Vector3.zero;
            item.localScale = Vector3.one;
        }

        public void SetChild(GameObject child)
        {
            child.transform.SetParent(_content);
            child.transform.localPosition = Vector3.zero;
        }
    }
}