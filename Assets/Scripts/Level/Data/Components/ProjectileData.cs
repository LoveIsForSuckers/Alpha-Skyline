using System;

namespace Assets.Scripts.Level.Data.Components
{
    public class ProjectileData
    {
        private uint _baseDamage = 0;
        private uint _damageVariance = 0;
        private bool _destroyOnCollision = true;
        private float _maxLifetime = 0;
        private float _lifetime = 0;

        public ProjectileData Load(ProjectileData otherData)
        {
            _baseDamage = otherData._baseDamage;
            _damageVariance = otherData._damageVariance;
            _destroyOnCollision = otherData._destroyOnCollision;
            _maxLifetime = otherData._maxLifetime;
            _lifetime = otherData._lifetime;
            HadCollision = false;
            return this;
        }

        public void Update(float deltaTime)
        {
            _lifetime += deltaTime;
        }

        public void Clear()
        {
            _baseDamage = _damageVariance = 0;
            _lifetime = _maxLifetime = 0;
            _destroyOnCollision = true;
            HadCollision = false;
        }
        
        public uint BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
        public uint DamageVariance { get { return _damageVariance; } set { _damageVariance = value; } }
        public float Lifetime { get { return _lifetime; } set { _lifetime = value; } }
        public float MaxLifetime { get { return _maxLifetime; } set { _maxLifetime = value; } }
        public bool DestroyOnCollision { get { return _destroyOnCollision; } set { _destroyOnCollision = value; } }
        public bool HadCollision { get; set; }
    }
}
