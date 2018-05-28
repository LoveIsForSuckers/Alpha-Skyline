using UnityEngine;

namespace Assets.Scripts.Level.Logic
{
    public interface IInputControlledEntity
    {
        void SetControlVector(Vector2 vector);
        Vector3 GetCurrentWorldPosition();
    }
}
