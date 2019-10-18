using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Transform colliderMarker;

        private FieldEntity _data;

        public void Init(FieldEntity data) {
            _data = data;
            sprite.enabled = true;
        }

        public void Update() {
            if ( _data == null ) {
                return;
            };

            transform.localPosition = _data.Movement.Position;
            colliderMarker.localScale = _data.Collision == null ? Vector3.zero : Vector3.one * _data.Collision.Radius * 2;
        }

        public void Clear() {
            _data = null;
            sprite.enabled = false;
            colliderMarker.localScale = Vector3.zero;
        }
    }
}
