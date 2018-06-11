using Assets.Scripts.Level.Data.Entity;
using UnityEngine;

namespace Assets.Scripts.Level.View
{
    public class PlayerShipView : MonoBehaviour
    {
        [SerializeField]
        private Transform colliderMarker;

        private FieldEntity _data;

        public void Init(FieldEntity data)
        {
            _data = data;
        }

        public void Update()
        {
            transform.localPosition = _data.Movement.Position;
            colliderMarker.localScale = _data.Collision == null ? Vector3.zero : Vector3.one * _data.Collision.Radius * 2;
        }
    }
}
