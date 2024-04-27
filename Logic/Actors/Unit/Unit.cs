using System;
using UnityEngine;
using DG.Tweening;

namespace Codebase.Logic
{
    public class Unit : MonoBehaviour
    {
        [SerializeField, Range(0f, 15f)] private float _speedWithCrystal = 7f;
        [SerializeField, Range(0f, 15f)] private float _speed = 4f;
        [SerializeField] private Transform _collectPoint;

        private Transform _target;
        private Vector3 _basePosition;

        public UnitStateTypes State { get; private set; } = UnitStateTypes.Standby;

        private void OnValidate()
        {
            if(_collectPoint == null)
                throw new ArgumentNullException(nameof(_collectPoint));
        }

        private void Start()
        {
            _basePosition = transform.position;
        }

        public void Collect(Crystal crystal)
        {
            State = UnitStateTypes.Collect;
            _target = crystal.transform;

            MoveToTargetAsync(_target.position, CollectCrystal);
        }

        private void MoveToTargetAsync(Vector3 position, Action<Transform> onComplete)
        {
            transform
                .DOMove(position, _speed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete.Invoke(_target));
        }

        private void ReturnToBaseAsync(Action onCompleted)
        {
            transform
                .DOMove(_basePosition, _speedWithCrystal)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() => onCompleted.Invoke());
        }

        private void CollectCrystal(Transform crystal)
        {
            crystal.SetParent(_collectPoint);
            crystal.localPosition = Vector3.zero;

            ReturnToBaseAsync(BaseReturned);
        }

        private void BaseReturned()
        {
            State = UnitStateTypes.Standby;
        }
    }
}
