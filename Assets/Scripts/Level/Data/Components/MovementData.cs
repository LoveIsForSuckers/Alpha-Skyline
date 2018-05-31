using UnityEngine;

namespace Assets.Scripts.Level.Data.Components
{
    public class MovementData
    {
        private Vector2 _position = Vector2.zero;
        private Vector2 _direction = Vector2.zero;
        private float _speed = 0;

        public void Update(float deltaTime)
        {
            _position += _direction * _speed * deltaTime;
        }

        public void Clear()
        {
            _position = _direction = Vector2.zero;
            _speed = 0;
        }

        public Vector2 Position { get { return _position; } set { _position = value; } }
        public Vector2 Direction { get { return _direction; } set { _direction = value.magnitude > 1 ? value.normalized : value ; } }
        public float Speed { get { return _speed; } set { _speed = value; } }
    }
}
