using Assets.Scripts.Level.Data.Entity;
using System;
using UnityEngine;

namespace Assets.Scripts.Level.Data.Components
{
    public class WeaponData
    {
        public event Action<WeaponData> Fired;
        
        private ProjectileData _projectile = null;
        private Vector2 _firePositionOffset = Vector2.zero;
        private Vector2 _fireDirection = Vector2.zero;
        private float _fireProjectileSpeed = 0;
        private float _rateOfFire = 0;
        private float _timeSinceLastShot = 0;

        public void Update(float deltaTime)
        {
            _timeSinceLastShot += deltaTime;

            if (_timeSinceLastShot > _rateOfFire)
            {
                DispatchFired();
                _timeSinceLastShot = 0;
            }
        }

        private void DispatchFired()
        {
            if (Fired != null)
                Fired(this);
        }

        public void Clear()
        {
            _projectile = null;
            _firePositionOffset = _fireDirection = Vector2.zero;
            _rateOfFire = _timeSinceLastShot = _fireProjectileSpeed = 0;
        }

        public bool IsCleared { get { return _projectile == null && _rateOfFire == 0; } }
        
        public ProjectileData Projectile { get { return _projectile; } set { _projectile = value; } }
        public float RateOfFire { get { return _rateOfFire; } set { _rateOfFire = value > 0 ? value : 0; } }
        public float FireProjectileSpeed { get { return _fireProjectileSpeed; } set { _fireProjectileSpeed = value > 0 ? value : 0; } }
        public Vector2 FirePositionOffset { get { return _firePositionOffset; } set { _firePositionOffset = value; } }
        public Vector2 FireDirection { get { return _fireDirection; } set { _fireDirection = value; } }
    }
}
