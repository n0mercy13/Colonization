using System;
using UnityEngine;
using VContainer;
using Codebase.StaticData;
using DG.Tweening;

namespace Codebase.Logic
{
    public class Crystal : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _renders;

        private Material[] _materials;
        private Color _endColor;
        private float _duration;

        [Inject]
        private void Construct(GameConfig gameConfig)
        {
            _endColor = gameConfig.CrystalScannedColor;
            _duration = 1.0f;
            
            _materials = new Material[_renders.Length];

            for(int i = 0; i < _materials.Length; i++)
                _materials[i] = _renders[i].material;
        }

        public bool IsScanned { get; private set; } = false;

        private void OnValidate()
        {
            if(_renders == null)
                throw new ArgumentNullException(nameof(_renders));
        }

        public void SetScanned()
        {
            IsScanned = true;
            ChangeColorAsync();
        }

        private void ChangeColorAsync()
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i]
                    .DOBlendableColor(_endColor, _duration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo); 
            }
        }
    }
}
