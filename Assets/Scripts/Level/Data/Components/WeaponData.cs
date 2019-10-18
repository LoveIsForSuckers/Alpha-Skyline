using Assets.Scripts.Level.Data.Entity;
using System;
using UnityEngine;

namespace Assets.Scripts.Level.Data.Components
{
    public class WeaponData
    {
        // TODO: should have not ProjectileData, but ProjectileLibItem instead
        private ProjectileData _projectile = null;
        private Vector2 _firePositionOffset = Vector2.zero;
        private Vector2 _fireDirection = Vector2.zero;
        private float _fireProjectileSpeed = 0;
        private float _inverseRateOfFire = 0;
        private float _timeSinceLastShot = 0;
        private bool _allowedToFire = true;

        public WeaponData Load(WeaponData otherData)
        {
            _projectile = otherData._projectile;
            _firePositionOffset = otherData._firePositionOffset;
            _fireDirection = otherData._fireDirection;
            _fireProjectileSpeed = otherData._fireProjectileSpeed;
            _inverseRateOfFire = otherData._inverseRateOfFire;
            _timeSinceLastShot = otherData._timeSinceLastShot;
            _allowedToFire = otherData._allowedToFire;
            return this;
        }

        public void Update(float deltaTime)
        {
            _timeSinceLastShot += deltaTime;
        }

        public void OnFired()
        {
            _timeSinceLastShot = 0;
        }

        public void Clear()
        {
            _projectile = null;
            _firePositionOffset = _fireDirection = Vector2.zero;
            _inverseRateOfFire = _timeSinceLastShot = _fireProjectileSpeed = 0;
        }

        public bool IsCleared { get { return _projectile == null && _inverseRateOfFire == 0; } }

        public bool IsCanFire { get { return _allowedToFire && _timeSinceLastShot >= _inverseRateOfFire; } }

        public ProjectileData Projectile { get { return _projectile; } set { _projectile = value; } }
        public float RateOfFire { get { return 1 / _inverseRateOfFire; } set { _inverseRateOfFire = value > 0 ? 1 / value : float.MaxValue; } }
        public float FireProjectileSpeed { get { return _fireProjectileSpeed; } set { _fireProjectileSpeed = value > 0 ? value : 0; } }
        public Vector2 FirePositionOffset { get { return _firePositionOffset; } set { _firePositionOffset = value; } }
        public Vector2 FireDirection { get { return _fireDirection; } set { _fireDirection = value; } }
        public bool AllowedToFire { get { return _allowedToFire; } set { _allowedToFire = value; } }
    }
}
