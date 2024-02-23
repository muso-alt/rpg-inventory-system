using Inventory.Views;

namespace Inventory.Events
{
    public struct BulletHitTargetEvent
    {
        public UnitView View;
        public int Damage;
        public ShootType ShootType;
    }
}