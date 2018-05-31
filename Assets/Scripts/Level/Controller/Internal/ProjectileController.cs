using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level.Controller.Internal
{
    public class ProjectileController
    {
        private const int DEFAULT_POOL_SIZE = 100;

        private Queue<FieldEntity> _pool;
        private List<ProjectileData> _active;

        public ProjectileController()
        {
            _active = new List<ProjectileData>();
            _pool = new Queue<FieldEntity>(DEFAULT_POOL_SIZE);

            for (int i = 0; i < DEFAULT_POOL_SIZE; i++)
            {
                var entity = new FieldEntity();
                entity.Projectile = new ProjectileData();
                _pool.Enqueue(entity);
            }
        }

        public FieldEntity CreateProjectile(ProjectileData data, Vector2 position, Vector2 direction)
        {
            var entity = GetEntity();
            entity.Projectile.Load(data);
            _active.Add(entity.Projectile);

            var movement = LevelController.Instance.Movement.Register(entity);
            movement.Position = position;
            movement.Direction = direction;

            return entity;
        }

        public void Unregister(FieldEntity projectileEntity)
        {
            var data = projectileEntity.Projectile;
            data.Clear();
            _active.Remove(data);

            LevelController.Instance.Movement.Unregister(projectileEntity);

            _pool.Enqueue(projectileEntity);
        }

        public FieldEntity GetEntity()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            else
            {
                var entity = new FieldEntity();
                entity.Projectile = new ProjectileData();
                return entity;
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var data in _active)
            {
                // TODO: check collisions here ?
            }
        }
    }
}
