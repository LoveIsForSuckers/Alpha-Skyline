using UnityEngine;

namespace Assets.Scripts.Level.View
{
    public class FieldBGView : MonoBehaviour
    {
        [SerializeField]
        private Material _material;
        [SerializeField]
        private float _scrollSpeed;

        private Vector2 _initialOffset;
        private float _offset;

        private void Start()
        {
            _initialOffset = _material.GetTextureOffset("_MainTex");
            _offset = 0.0f;
        }

        private void Update()
        {
            _offset += Time.deltaTime * _scrollSpeed;
            _offset = Mathf.Repeat(_offset, 1.0f);
            _material.SetTextureOffset("_MainTex", _initialOffset + new Vector2(0, _offset));
        }

        private void OnDisable()
        {
            Clear();
        }

        private void Clear()
        {
            _material.SetTextureOffset("_MainTex", _initialOffset);
        }
    }
}
