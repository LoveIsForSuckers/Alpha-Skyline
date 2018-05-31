using Assets.Scripts.Level.Data.Entity;

namespace Assets.Scripts.Level.Controller
{
    public interface ILevelListener
    {
        void OnProjectileCreated(FieldEntity projectileEntity);
        void OnProjectileDestroying(FieldEntity projectileEntity);
    }
}
