using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level.Controller.Internal
{
    public class ProjectileController
    {
        public event Action<FieldEntity> EntityCreated;
        public event Action<FieldEntity> EntityDestroying;

        private const int DEFAULT_POOL_SIZE = 100;

        private Queue<FieldEntity> _pool;
        private Dictionary<ProjectileData, FieldEntity> _active;

        public ProjectileController()
        {
            _active = new Dictionary<ProjectileData, FieldEntity>();
            _pool = new Queue<FieldEntity>(DEFAULT_POOL_SIZE);

            for (int i = 0; i < DEFAULT_POOL_SIZE; i++)
            {
                var entity = new FieldEntity();
                entity.Name = "Proj";
                entity.Projectile = new ProjectileData();
                _pool.Enqueue(entity);
            }
        }

        // TODO: oh boi we need factories everywhere
        public FieldEntity CreateProjectile(ProjectileData data)
        {
            var entity = GetNewEntity();
            entity.Projectile.Load(data);
            _active.Add(entity.Projectile, entity);

            LevelController.Instance.Movement.Register(entity);
            LevelController.Instance.Collision.Register(entity);

            if (EntityCreated != null)
                EntityCreated(entity);

            return entity;
        }
        
        public void Unregister(FieldEntity projectileEntity)
        {
            if (EntityDestroying != null)
                EntityDestroying(projectileEntity);

            var data = projectileEntity.Projectile;
            data.Clear();
            _active.Remove(data);

            LevelController.Instance.Movement.Unregister(projectileEntity);
            LevelController.Instance.Collision.Unregister(projectileEntity);

            _pool.Enqueue(projectileEntity);
        }

        public FieldEntity GetNewEntity()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            else
            {
                var entity = new FieldEntity();
                entity.Name = "Proj";
                entity.Projectile = new ProjectileData();
                return entity;
            }
        }

        public void Update(float deltaTime)
        {
            // TODO: optimize
            var expiredProjectiles = new List<FieldEntity>();

            foreach (var kv in _active)
            {
                var data = kv.Key;
                data.Update(deltaTime);
                if (data.Lifetime > data.MaxLifetime || (data.DestroyOnCollision && data.HadCollision))
                    expiredProjectiles.Add(kv.Value);
            }

            foreach (var entity in expiredProjectiles)
            {
                Unregister(entity);
            }

            expiredProjectiles.Clear();
        }

        public void HandleCollision(FieldEntity entity1, FieldEntity entity2) {
            if ( entity1.Projectile != null ) {
                entity1.Projectile.HadCollision = true;
            }
            if ( entity2.Projectile != null ) {
                entity2.Projectile.HadCollision = true;
            }
        }
    }
}
