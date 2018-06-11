using Assets.Scripts.Level.Logic;
using UnityEngine;

namespace Assets.Scripts.Level.Data.Entity
{
    public class InputControlledEntity : FieldEntity, IInputControllable
    {
        public Vector2 GetCurrentPosition()
        {
            if (Movement != null)
                return Movement.Position;
            else
                return Vector2.zero;
        }

        public void SetControlVector(Vector2 vector)
        {
            if (Movement != null)
                Movement.Direction = vector;
        }

        public void SetAllowedToFire(bool value)
        {
            foreach (var weapon in Weapons)
                weapon.AllowedToFire = value;
        }
    }
}
