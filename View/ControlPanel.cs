using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Codebase.View
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private Button _scanButton;
        [SerializeField] private Button _collectButton;
        [SerializeField] private TMP_Text _unitsLabel;
        [SerializeField] private TMP_Text _crystalsLabel;
        [Header("ButtonAnimation")]
        [SerializeField, Range(1f, 1.5f)] private float _endScale;
        [SerializeField, Range(0.1f, 1.5f)] private float _duration; 

        private Tweener _scanButtonAnimation;
        private Tweener _collectButtonAnimation;

        public event Action<bool> CollectPressed = delegate { };

        public bool IsScanActive { get; private set; }
        public bool IsCollectActive { get; private set; }

        private void OnValidate()
        {
            if (_scanButton == null)
                throw new ArgumentNullException(nameof(_scanButton));

            if (_collectButton == null)
                throw new ArgumentNullException(nameof(_collectButton));

            if (_unitsLabel == null)
                throw new ArgumentNullException(nameof(_unitsLabel));

            if (_crystalsLabel == null)
                throw new ArgumentNullException(nameof(_crystalsLabel));
        }

        private void OnEnable()
        {
            _scanButton.onClick.AddListener(OnScanPressed);
            _collectButton.onClick.AddListener(OnCollectPressed);
        }

        private void OnDisable()
        {
            _scanButton.onClick.RemoveListener(OnScanPressed);
            _collectButton.onClick.RemoveListener(OnCollectPressed);

            _scanButtonAnimation.Kill();
            _collectButtonAnimation.Kill();
        }

        public void UpdateCrystalInfo(int crystalsCount) =>
            _crystalsLabel.text = $"Crystals : {crystalsCount:D3}";

        public void StopCollectAnimation()
        {
            IsCollectActive = false;
            _collectButtonAnimation.Kill();
        }

        private void OnCollectPressed()
        {
            if (IsCollectActive)
                _collectButtonAnimation.Kill();
            else
                _collectButtonAnimation = AnimateButtonAsync(_collectButton);

            IsCollectActive = !IsCollectActive;
            CollectPressed.Invoke(IsCollectActive);
        }

        private void OnScanPressed()
        {
            if(IsScanActive)
                _scanButtonAnimation.Kill();
            else
                _scanButtonAnimation = AnimateButtonAsync(_scanButton);

            IsScanActive = !IsScanActive;
        }

        private Tweener AnimateButtonAsync(Button button) => button.transform
                .DOScale(Vector2.one * _endScale, _duration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
    }
}
