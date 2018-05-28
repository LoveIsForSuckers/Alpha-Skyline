using UnityEngine;

namespace Assets.Scripts.Level.Logic
{
    public class InputControlledEntity : MonoBehaviour, IInputControlledEntity
    {
        private const float BOUNDS_WIDTH_MALUS = 0.75f;

        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Rect _moveBounds;
        [SerializeField]
        private float _moveSpeed;

        private Vector2 _controlVector;

        private void Awake()
        {
            var aspect = (float)Screen.width / Screen.height;
            var unitsWidth = _moveBounds.height * aspect - BOUNDS_WIDTH_MALUS;
            _moveBounds.width = unitsWidth;
            _moveBounds.x = -0.5f * unitsWidth;
        }

        private void Update()
        {
            var currentPos = transform.localPosition;
            var deltaPos = Vector2.ClampMagnitude(_controlVector, _moveSpeed);
            _controlVector -= deltaPos;
            currentPos.x = Mathf.Clamp(currentPos.x + deltaPos.x, _moveBounds.xMin, _moveBounds.xMax);
            currentPos.y = Mathf.Clamp(currentPos.y + deltaPos.y, _moveBounds.yMin, _moveBounds.yMax);
            transform.localPosition = currentPos;
        }

        public Vector3 GetCurrentWorldPosition()
        {
            return transform.position;
        }

        public void SetControlVector(Vector2 vector)
        {
            _controlVector = vector;
        }
    }
}
