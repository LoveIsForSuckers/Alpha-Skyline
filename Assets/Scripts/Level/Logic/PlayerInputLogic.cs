using Assets.Scripts.Level.Controller;
using UnityEngine;

namespace Assets.Scripts.Level.Logic
{
    public class PlayerInputLogic : MonoBehaviour
    {
        public static PlayerInputLogic Instance; // TODO: hack for now, need integrate with LevelMainController

        [SerializeField]
        private Camera _camera;

        private Rect _moveBounds;
        
        public IInputControllable entity;

        public PlayerInputLogic()
        {
            Instance = this; // TODO: hack for now, need integrate with LevelMainController
        }

        private void Awake()
        {
            new LevelMainController(); // TODO: hack for now, need LevelMainBehaviour

            var aspect = _camera.aspect;
            var size = _camera.orthographicSize;

            _moveBounds = new Rect(-size * aspect, -size, 2 * size * aspect, 2 * size);
        }

        private void Update()
        {
            if (entity == null)
                return;

            if (Input.GetMouseButton(0))
            {
                var targetPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                targetPos.z = 0;

                // clamp and bounds are useless for mouse controlled input but may be useful later
                targetPos.x = Mathf.Clamp(targetPos.x, _moveBounds.xMin, _moveBounds.xMax);
                targetPos.y = Mathf.Clamp(targetPos.y, _moveBounds.yMin, _moveBounds.yMax);

                var currentPos = entity.GetCurrentPosition();
                var controlVector = new Vector2(targetPos.x, targetPos.y) - currentPos;
                entity.SetControlVector(controlVector);
            }
            else
            {
                entity.SetControlVector(Vector2.zero);
            }

            LevelMainController.Instance.movementController.Update(Time.deltaTime); // TODO: hack for now, need LevelMainBehaviour
        }
    }
}