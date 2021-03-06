﻿using Assets.Scripts.Level.Controller;
using Assets.Scripts.Level.Data.Entity;
using UnityEngine;

namespace Assets.Scripts.Level.Logic
{
    public class PlayerInputLogic : MonoBehaviour
    {
        public static PlayerInputLogic Instance; // TODO: hack for now, need integrate with LevelMainController

        [SerializeField]
        private Camera _camera;
        
        public IInputControllable entity;

        public PlayerInputLogic()
        {
            Instance = this; // TODO: hack for now, need integrate with LevelMainController
        }

        private void Awake()
        {
            new LevelController(); // TODO: hack for now, need LevelMainBehaviour

            var aspect = _camera.aspect;
            var size = _camera.orthographicSize;

            LevelController.Instance.Movement.PlayableAreaBounds = new Rect(-size * aspect, -size, 2 * size * aspect, 2 * size); // TODO: hack for now
        }

        private void Update()
        {
            if (entity == null)
                return;
            
            var targetPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            var moveBounds = LevelController.Instance.Movement.PlayableAreaBounds;
                
            targetPos.x = Mathf.Clamp(targetPos.x, moveBounds.xMin, moveBounds.xMax);
            targetPos.y = Mathf.Clamp(targetPos.y, moveBounds.yMin, moveBounds.yMax);

            var normSpeed = (entity as FieldEntity).Movement.Speed * Time.fixedDeltaTime * Time.timeScale;
                
            var currentPos = entity.GetCurrentPosition();
            var controlVector = new Vector2(targetPos.x, targetPos.y) - currentPos;
            if (controlVector.magnitude < normSpeed)
                controlVector *= controlVector.magnitude / normSpeed;
            entity.SetControlVector(controlVector);

            entity.SetAllowedToFire(Input.GetMouseButton(0));
        }

        private void FixedUpdate()
        {
            LevelController.Instance.Update(Time.fixedDeltaTime * Time.timeScale); // TODO: hack for now, need LevelMainBehaviour
        }
    }
}