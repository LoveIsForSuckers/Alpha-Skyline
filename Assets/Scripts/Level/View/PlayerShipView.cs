using Assets.Scripts.Level.Data.Entity;
using UnityEngine;

namespace Assets.Scripts.Level.View
{
    public class PlayerShipView : MonoBehaviour
    {
        public Vector2 DEBUG_DIRECTION;
        public Vector2 DEBUG_POSITION;

        private FieldEntity _data;

        public void Init(FieldEntity data)
        {
            _data = data;
        }

        public void Update()
        {
            transform.localPosition = _data.Movement.Position;
            DEBUG_DIRECTION = _data.Movement.Direction;
            DEBUG_POSITION = _data.Movement.Position;
        }
    }
}
