using UnityEngine;

namespace Assets.Scripts.Level.Logic
{
    public class PlayerInputLogic : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField] // TODO: replace with IInputControlledEntity
        private InputControlledEntity _entity;

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var targetPos = _camera.ScreenToWorldPoint(Input.mousePosition);
                targetPos.z = 0;
                var currentPos = _entity.GetCurrentWorldPosition();
                currentPos.z = 0;

                var controlVector = targetPos - currentPos;
                _entity.SetControlVector(controlVector);
            }
        }
    }
}