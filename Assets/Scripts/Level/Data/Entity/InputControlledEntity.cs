using Assets.Scripts.Level.Logic;
using UnityEngine;

namespace Assets.Scripts.Level.Data.Entity
{
    public class InputControlledEntity : FieldEntity, IInputControllable
    {
        public Vector2 GetCurrentPosition()
        {
            if (movement != null)
                return movement.Position;
            else
                return Vector2.zero;
        }

        public void SetControlVector(Vector2 vector)
        {
            if (movement != null)
                movement.Direction = vector;
        }
    }
}
