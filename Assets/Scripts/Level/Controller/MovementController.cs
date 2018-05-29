using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System.Collections.Generic;

namespace Assets.Scripts.Level.Controller
{
    public class MovementController
    {
        private const int DEFAULT_POOL_SIZE = 50;

        private Queue<MovementData> _pooledMovements;
        private List<MovementData> _activeMovements;

        public MovementController()
        {
            _activeMovements = new List<MovementData>();
            _pooledMovements = new Queue<MovementData>(DEFAULT_POOL_SIZE);
            for (int i = 0; i < DEFAULT_POOL_SIZE; i++)
                _pooledMovements.Enqueue(new MovementData());
        }

        public MovementData RegisterMovable(FieldEntity entity)
        {
            var data = GetMovementData();
            entity.movement = data;
            _activeMovements.Add(data);
            return data;
        }

        public void UnregisterMovable(FieldEntity entity)
        {
            var data = entity.movement;
            data.Clear();
            _activeMovements.Remove(data);
            _pooledMovements.Enqueue(data);
            entity.movement = null;
        }

        private MovementData GetMovementData()
        {
            if (_pooledMovements.Count > 0)
                return _pooledMovements.Dequeue();
            else
                return new MovementData();
        }

        public void Update(float deltaTime)
        {
            foreach (var data in _activeMovements)
                data.Update(deltaTime);
        }
    }
}
