using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.Controller.Internal
{
    public class CollisionController
    {
        public event Action<FieldEntity, FieldEntity, Vector2> CollisionDetected;

        private const int DEFAULT_POOL_SIZE = 150;

        private Dictionary<CollisionData, FieldEntity> _collisionOwners;
        private Queue<CollisionData> _pool;
        private List<CollisionData> _active;

        public CollisionController()
        {
            _collisionOwners = new Dictionary<CollisionData, FieldEntity>();
            _active = new List<CollisionData>();
            _pool = new Queue<CollisionData>(DEFAULT_POOL_SIZE);
            for (int i = 0; i < DEFAULT_POOL_SIZE; i++)
                _pool.Enqueue(new CollisionData());
        }

        public CollisionData Register(FieldEntity entity)
        {
            var data = GetCollisionData();
            entity.Collision = data;
            _active.Add(data);
            _collisionOwners.Add(data, entity);
            return data;
        }

        public void Unregister(FieldEntity entity)
        {
            var data = entity.Collision;
            _active.Remove(data);
            _collisionOwners.Remove(data);
            data.Radius = 0;
            _pool.Enqueue(data);
            entity.Collision = null;
        }

        private CollisionData GetCollisionData()
        {
            if (_pool.Count > 0)
                return _pool.Dequeue();
            else
                return new CollisionData();
        }

        public void Update(float deltaTime)
        {
            var checkedCollisions = new Dictionary<CollisionData, List<CollisionData>>(); // TODO: allocations on update cmon
            foreach (var collision in _active)
                checkedCollisions[collision] = new List<CollisionData>(); // TODO: allocations on update cmon

            // TODO: o(n^2), optimize if needed
            foreach (var collision1 in _active)
            {
                var entity1 = _collisionOwners[collision1];

                foreach (var collision2 in _active)
                {
                    if (collision1 == collision2 || checkedCollisions[collision1].Contains(collision2) || checkedCollisions[collision2].Contains(collision1))
                        continue;

                    checkedCollisions[collision1].Add(collision2);

                    var entity2 = _collisionOwners[collision2];

                    if (CheckNonCollidable(entity1, entity2))
                        continue;

                    CheckCollision(entity1, entity2, deltaTime);
                }
            }
        }

        private void CheckCollision(FieldEntity entity1, FieldEntity entity2, float deltaTime)
        {
            // Check here for algorithm explanation:
            // https://www.gamasutra.com/view/feature/131424/pool_hall_lessons_fast_accurate_.php?page=2
            // Slightly modified to include lasting collisions

            var movement1 = entity1.Movement;
            var movement2 = entity2.Movement;
            var collision1 = entity1.Collision;
            var collision2 = entity2.Collision;

            // 0. movementVector calculation in movement1 space (so movement1 is 'static')
            var movementVector = (movement2.Direction * movement2.Speed - movement1.Direction * movement1.Speed);
            var movementDistance = movementVector.magnitude * deltaTime;

            // 1. distance check
            var currentDistance = Vector2.Distance(movement1.Position, movement2.Position);
            var radiusSum = collision1.Radius + collision2.Radius;
            if (movementDistance < currentDistance - radiusSum)
                return;
            
            // 2. move direction check
            var normalizedMovementVector = movementVector.normalized;
            var from1To2Vector = movement1.Position - movement2.Position;
            var dotProduct = Vector2.Dot(from1To2Vector, normalizedMovementVector);
            if (dotProduct <= 0) // moving away
                return;

            // 3. escape check
            var from1To2VectorSqrMag = from1To2Vector.sqrMagnitude;
            var futureCentersYSqrMag = from1To2VectorSqrMag - dotProduct * dotProduct;
            if (futureCentersYSqrMag >= radiusSum * radiusSum)
                return;

            // 4. triangle check
            var futureCentersXSqrMag = radiusSum * radiusSum - futureCentersYSqrMag;
            if (futureCentersXSqrMag < 0)
                return;

            // 5. future distance check
            var futureDistance = dotProduct - Mathf.Sqrt(futureCentersXSqrMag);
            if (movementDistance < futureDistance)
                return;

            // 6. collision detected for sure, getting point
            movementVector = movementVector.normalized * futureDistance;
            movementVector += movement1.Direction * movement1.Speed * deltaTime; // converting back to global space
            var lerpFactor = collision1.Radius / radiusSum;
            var collisionPosition = Vector2.Lerp(movement1.Position, movement2.Position + movementVector, lerpFactor);
            OnCollisionDetected(entity1, entity2, collisionPosition);
        }

        private void OnCollisionDetected(FieldEntity entity1, FieldEntity entity2, Vector2 collisionPosition)
        {
            if (CollisionDetected != null)
                CollisionDetected(entity1, entity2, collisionPosition);
        }

        private bool CheckNonCollidable(FieldEntity entity1, FieldEntity entity2)
        {
            return (entity1.Projectile != null && entity2.Projectile != null) ||
                    (entity1.Collision.IsPlayerOwned == entity2.Collision.IsPlayerOwned);
        }
    }
}
