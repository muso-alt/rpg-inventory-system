using Inventory.Data;
using Inventory.Views;

namespace Inventory.Components
{
    public struct Gun
    {
        public AmmoType Type;
        public int BulletSpendCount;
        public GunView View;
    }
}