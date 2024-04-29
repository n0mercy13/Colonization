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

        public event Action CrystalDelivered = delegate { };

        public UnitStateTypes State { get; private set; } = UnitStateTypes.Standby;

        private void OnValidate()
        {
            if(_collectPoint == null)
                throw new ArgumentNullException(nameof(_collectPoint));
        }

        public void Collect(Crystal crystal)
        {
            State = UnitStateTypes.Collect;
            _basePosition = transform.position;
            _target = crystal.transform;

            MoveToTargetAsync(_target.position, CollectCrystal);
        }

        public void BuildBase(Vector3 position, Func<Vector3, Base> buildBase)
        {
            MoveToTargetAsync(position, buildBase);
        }

        private void MoveToTargetAsync(Vector3 position, Action<Transform> onComplete)
        {
            transform
                .DOMove(position, _speed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete.Invoke(_target));
        }

        private void MoveToTargetAsync(Vector3 position, Func<Vector3, Base> onComplete)
        {
            transform
                .DOMove(position, _speed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete
                .Invoke(position)
                .Register(this));
        }

        private void CollectCrystal(Transform crystal)
        {
            crystal.SetParent(_collectPoint);
            crystal.localPosition = Vector3.zero;

            MoveToTargetAsync(_basePosition ,BaseReturned);
        }

        private void BaseReturned(Transform crystal)
        {
            State = UnitStateTypes.Standby;
            Destroy(crystal.gameObject);
            CrystalDelivered.Invoke();
        }
    }
}
