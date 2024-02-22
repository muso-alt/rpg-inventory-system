using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Components;
using Inventory.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace Inventory.Services
{
    public static class Extensions
    {
        public static ref Item GetItemByView(this ItemView view)
        {
            if (!view.PackedEntityWithWorld.Unpack(out var world, out var targetEntity))
            {
                throw new System.NullReferenceException();
            }
                
            var unitPool = world.GetPool<Item>();
            return ref unitPool.Get(targetEntity);
        }
        
        public static bool IsInsideOtherRectByPosition(this RectTransform other, Vector3 position)
        {
            var uiObjectCenter = position;

            var parentCorners = new Vector3[4];
            other.GetWorldCorners(parentCorners);

            var parentBottomLeft = parentCorners[0];
            var parentTopRight = parentCorners[2];

            return uiObjectCenter.x > parentBottomLeft.x &&
                   uiObjectCenter.x < parentTopRight.x &&
                   uiObjectCenter.y > parentBottomLeft.y &&
                   uiObjectCenter.y < parentTopRight.y;
        }

        public static CellView GetItemCell(this ItemView view, IEnumerable<CellView> cells)
        {
            return cells.Where(cell => cell.ChildItem != null).FirstOrDefault(cell => cell.ChildItem == view);
        }
    }
}