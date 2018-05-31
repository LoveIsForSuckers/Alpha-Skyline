using Assets.Scripts.Level.Data.Components;
using System.Collections.Generic;

namespace Assets.Scripts.Level.Data.Entity
{
    public class FieldEntity
    {
        public List<WeaponData> Weapons { get; private set; }
        public MovementData Movement { get; set; }
        public ProjectileData Projectile { get; set; }

        public FieldEntity()
        {
            Weapons = new List<WeaponData>();
        }
    }
}
