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
            data.Fired += OnFired;
            entity.Weapons.Add(data);
            _active.Add(data);

            _weaponOwners.Add(data, entity);

            return data;
        }

        public void UnregisterWeapon(FieldEntity entity, WeaponData data)
        {
            if (entity.Weapons.Remove(data))
            {
                data.Fired -= OnFired;
                data.Clear();
                _active.Remove(data);

                _weaponOwners.Remove(data);
            }
        }
        
        public void Update(float deltaTime)
        {
            foreach (var data in _active)
                data.Update(deltaTime);
        }

        private void OnFired(WeaponData data)
        {
            var position = data.FirePositionOffset + _weaponOwners[data].Movement.Position;
            LevelController.Instance.Projectile.CreateProjectile(data.Projectile, position, data.FireDirection);
        }
    }
}
