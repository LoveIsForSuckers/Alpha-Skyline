using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Level.Controller.Internal
{
    public class WeaponController
    {
        private Dictionary<WeaponData, FieldEntity> _weaponOwners;
        private List<WeaponData> _active;

        public WeaponController()
        {
            _active = new List<WeaponData>();
            _weaponOwners = new Dictionary<WeaponData, FieldEntity>();
        }

        public WeaponData RegisterWeapon(FieldEntity entity, WeaponData data)
        {
            entity.Weapons.Add(data);
            _active.Add(data);

            _weaponOwners.Add(data, entity);

            return data;
        }

        public void UnregisterWeapon(FieldEntity entity, WeaponData data)
        {
            if (entity.Weapons.Remove(data))
            {
                data.Clear();
                _active.Remove(data);

                _weaponOwners.Remove(data);
            }
        }
        
        public void Update(float deltaTime)
        {
            foreach (var data in _active)
            {
                data.Update(deltaTime);
                if (data.IsCanFire)
                {
                    var weaponOwner = _weaponOwners[data];

                    data.OnFired();

                    var projectilePosition = data.FirePositionOffset + weaponOwner.Movement.Position;
                    var projectileEntity = LevelController.Instance.Projectile.CreateProjectile(data.Projectile);
                    projectileEntity.Movement.Position = projectilePosition;
                    projectileEntity.Movement.Direction = data.FireDirection;
                    projectileEntity.Movement.Speed = data.FireProjectileSpeed;
                    projectileEntity.Collision.IsPlayerOwned = weaponOwner.Collision == null ? true : weaponOwner.Collision.IsPlayerOwned;
                    projectileEntity.Collision.Radius = 0.1f; // TODO: HACK
                }
            }
        }
    }
}
