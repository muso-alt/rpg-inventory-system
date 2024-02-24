using Inventory.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Inventory.Systems
{
    public class ItemCountDisplaySystem : IEcsRunSystem
    {
        private EcsPoolInject<Item> _itemsPool;
        private EcsFilterInject<Inc<Item>> _itemFilter;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _itemFilter.Value)
            {
                ref var item = ref _itemsPool.Value.Get(entity);

                if (item.View == null)
                {
                    continue;
                }

                item.View.ToggleCountText(item.CurrentCount > 1);
                item.View.SetCountText(item.CurrentCount);
            }
        }
    }
}