using Assets.Scripts.Level.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Level.Controller.Enemies
{
    /// <summary>
    /// Controls a single enemy
    /// </summary>
    public class EnemyController
    {
        private FieldEntity _entity;

        private Vector2 _targetPos;

        public void Load(FieldEntity entity)
        {
            _entity = entity;
            _targetPos = _entity.Movement.Position;
        }

        public void Update(float deltaTime)
        {
            if (_entity == null)
                return;

            if (Vector2.SqrMagnitude(_entity.Movement.Position - _targetPos) < 0.01f)
                UpdateTarget();

            if ( _entity.Weapons != null ) {
                foreach ( var weapon in _entity.Weapons ) {
                    weapon.AllowedToFire = true;
                }
            }
        }

        private void UpdateTarget()
        {
            var moveBounds = LevelController.Instance.Movement.PlayableAreaBounds;

            if (_targetPos.x < 0)
                _targetPos.x = 0.9f * moveBounds.xMax; // assuming xMax > 0
            else
                _targetPos.x = 0.9f * moveBounds.xMin; // assuming xMin < 0

            _entity.Movement.Direction = _targetPos - _entity.Movement.Position;
        }

        public bool IsThis(FieldEntity entity) {
            return entity == _entity; // TODO: check ids instead?
        }

        public void Clear() {
            _entity = null;
        }
    }
}
