using System;
using UnityEngine;
using DG.Tweening;

namespace Codebase.Logic
{
    public class BuildMarker : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField, Range(0f, 2f)] private float _duration;
        [SerializeField] private Color _endColor;
        [SerializeField] private Vector3 _endScale;

        private void OnValidate()
        {
            if(_renderer == null)
                throw new ArgumentNullException(nameof(_renderer));
        }

        private void Start()
        {
            ChangeScaleAsync();
            ChangeColorAsync();
        }

        public void UnHide()
        {
            gameObject.SetActive(true);
        }

        public void Hide() =>
            gameObject.SetActive(false);

        private void ChangeColorAsync()
        {
            _renderer.material
                .DOBlendableColor(_endColor, _duration)
                .SetEase(Ease.InSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void ChangeScaleAsync()
        {
            transform
                .DOScale(_endScale, _duration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
