using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Components;
using Inventory.Data;
using Inventory.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Inventory.Services
{
    public static class Extensions
    {
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
        
        public static bool TryGetAmmoEntityByType(this EcsPoolInject<Ammo> ammoPool, 
            EcsFilterInject<Inc<Ammo>> filter, AmmoType type, out int id)
        {
            foreach (var entity in filter.Value)
            {
                ref var ammo = ref ammoPool.Value.Get(entity);

                if (ammo.Type != type)
                {
                    continue;
                }

                id = entity;
                return true;
            }

            id = 0;
            return false;
        }

        public static CellView GetRandomEmptyCell(this IEnumerable<CellView> cells)
        {
            var emptyCells = cells.Where(cell => cell.ChildItem == null).ToList();
            return emptyCells[Random.Range(0, emptyCells.Count)];
        }
    }
}