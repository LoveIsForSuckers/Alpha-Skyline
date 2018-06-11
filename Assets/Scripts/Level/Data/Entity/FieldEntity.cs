using Assets.Scripts.Level.Data.Components;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Level.Data.Entity
{
    public class FieldEntity
    {
        public string Name { get; set; }
        public List<WeaponData> Weapons { get; private set; }
        public MovementData Movement { get; set; }
        public ProjectileData Projectile { get; set; }
        public CollisionData Collision { get; set; }

        public FieldEntity()
        {
            Weapons = new List<WeaponData>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Name);
            sb.Append('_');
            if (Weapons.Count > 0)
                sb.Append("W" + Weapons.Count);
            if (Movement != null)
                sb.Append('M');
            if (Projectile != null)
                sb.Append('P');
            if (Collision != null)
                sb.Append('C');

            return sb.ToString();
        }
    }
}
