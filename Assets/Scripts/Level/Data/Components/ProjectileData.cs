namespace Assets.Scripts.Level.Data.Components
{
    public class ProjectileData
    {
        private float _lifetime = 0;
        private float _baseDamage = 0;
        private float _damageVariance = 0;
        private bool _destroyOnImpact = true;

        public void Load(ProjectileData otherData)
        {
            _lifetime = otherData._lifetime;
            _baseDamage = otherData._baseDamage;
            _damageVariance = otherData._damageVariance;
            _destroyOnImpact = otherData._destroyOnImpact;
        }

        public void Clear()
        {
            _lifetime = _baseDamage = _damageVariance = 0;
            _destroyOnImpact = true;
        }

        public float Lifetime { get { return _lifetime; } set { _lifetime = value; } }
        public float BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
        public float DamageVariance { get { return _damageVariance; } set { _damageVariance = value; } }
        public bool DestroyOnImpact { get { return _destroyOnImpact; } set { _destroyOnImpact = value; } }
    }
}
