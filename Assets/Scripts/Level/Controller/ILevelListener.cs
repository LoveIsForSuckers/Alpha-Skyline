using Assets.Scripts.Level.Data.Entity;
using UnityEngine;

namespace Assets.Scripts.Level.Controller
{
    public interface ILevelListener
    {
        void OnProjectileCreated(FieldEntity projectileEntity);
        void OnProjectileDestroying(FieldEntity projectileEntity);
        void OnCollisionDetected(FieldEntity entity1, FieldEntity entity2, Vector2 position);
    }
}
