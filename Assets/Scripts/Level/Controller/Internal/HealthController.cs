using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Level.Controller.Internal
{
    public class HealthController
    {
        private const uint HealthOwnersCollisionDamage = 10;

        public event Action<FieldEntity> EntityShouldDie;

        private Dictionary<HealthData, FieldEntity> _healthOwners;
        private List<HealthData> _active;

        public HealthController() {
            _active = new List<HealthData>();
            _healthOwners = new Dictionary<HealthData, FieldEntity>();
        }

        public HealthData Register(FieldEntity entity) {
            var data = new HealthData(); // TODO: pool theze boiz
            entity.Health = data;
            _active.Add(data);

            _healthOwners.Add(data, entity);

            return data;
        }

        public void Unregister(FieldEntity entity) {
            var data = entity.Health;
            _active.Remove(data);
            _healthOwners.Remove(data);
            data.Clear();
            entity.Health = null;
        }

        public void HandleCollision(FieldEntity entity1, FieldEntity entity2) {
            var firstIsProjectile = entity1.Projectile != null;
            var secondIsProjectile = entity2.Projectile != null;
            var firstHasHealth = entity1.Health != null;
            var secondHasHealth = entity2.Health != null;

            if ( firstIsProjectile ) {
                if ( secondHasHealth ) {
                    entity2.Health.TakeDamage(RollDamage(entity1.Projectile));
                }
            } else if ( secondIsProjectile ) {
                if ( firstHasHealth ) {
                    entity1.Health.TakeDamage(RollDamage(entity2.Projectile));
                }
            } else if ( firstHasHealth && secondHasHealth ) {
                entity1.Health.TakeDamage(HealthOwnersCollisionDamage);
                entity2.Health.TakeDamage(HealthOwnersCollisionDamage);
            }

            if ( EntityShouldDie != null ) {
                if ( firstHasHealth && entity1.Health.CurrentHealth == 0 ) {
                    EntityShouldDie(entity1);
                }
                if ( secondHasHealth && entity2.Health.CurrentHealth == 0 ) {
                    EntityShouldDie(entity2);
                }
            }
        }

        private uint RollDamage(ProjectileData projectile) {
            return RollDamage(projectile.BaseDamage, projectile.DamageVariance);
        }

        private uint RollDamage(uint baseDamage, uint variance) {
            var rand = UnityEngine.Random.Range(( int ) -variance, ( int ) variance);
            var intVal = baseDamage + rand;
            return intVal > 0 ? ( uint ) intVal : 0;
        }
    }
}
