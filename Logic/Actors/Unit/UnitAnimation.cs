using System;
using UnityEngine;
using DG.Tweening;

namespace Codebase.Logic
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField, Range(0f, 5f)] private float _idleDuration;
        [SerializeField, Range(0f, 2f)] private float _idleAmplitude;

        private float _endValueIdle;

        private void OnValidate()
        {
            if (_transform == null)
                throw new ArgumentNullException(nameof(_transform));
        }

        private void Awake()
        {
            _endValueIdle = _transform.position.y - _idleAmplitude;
        }

        private void Start()
        {
            IdleAsync();
        }

        private void OnDisable()
        {
            _transform.DOKill();
        }

        private void IdleAsync()
        {
            _transform
                .DOMoveY(_endValueIdle, _idleDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    } 
}
