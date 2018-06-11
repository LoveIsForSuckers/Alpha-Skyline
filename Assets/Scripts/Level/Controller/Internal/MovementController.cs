using Assets.Scripts.Level.Data.Components;
using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level.Controller.Internal
{
    public class MovementController
    {
        private const int DEFAULT_POOL_SIZE = 150;

        private Queue<MovementData> _pooledMovements;
        private List<MovementData> _activeMovements;
        private Rect _playableAreaBounds;

        public MovementController()
        {
            _activeMovements = new List<MovementData>();
            _pooledMovements = new Queue<MovementData>(DEFAULT_POOL_SIZE);
            for (int i = 0; i < DEFAULT_POOL_SIZE; i++)
                _pooledMovements.Enqueue(new MovementData());
        }

        public MovementData Register(FieldEntity entity)
        {
            var data = GetMovementData();
            entity.Movement = data;
            _activeMovements.Add(data);
            return data;
        }

        public void Unregister(FieldEntity entity)
        {
            var data = entity.Movement;
            data.Clear();
            _activeMovements.Remove(data);
            _pooledMovements.Enqueue(data);
            entity.Movement = null;
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

        public Rect PlayableAreaBounds { get { return _playableAreaBounds; } set { _playableAreaBounds = value; } }
    }
}
