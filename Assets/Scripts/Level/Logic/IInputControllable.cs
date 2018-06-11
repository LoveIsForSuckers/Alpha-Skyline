using UnityEngine;

namespace Assets.Scripts.Level.Logic
{
    public interface IInputControllable
    {
        void SetControlVector(Vector2 vector);
        Vector2 GetCurrentPosition();
        void SetAllowedToFire(bool value);
    }
}
