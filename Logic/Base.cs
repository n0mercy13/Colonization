using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Codebase.Infrastructure;

namespace Codebase.Logic
{
    [RequireComponent(typeof(Collider))]
    public class Base : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        private IGameFactory _gameFactory;
        private Coroutine _collectCrystalsCoroutine;
        private YieldInstruction _checkDelay;
        private List<Unit> _units = new();
        private Stack<Crystal> _scannedCrystals = new();
        private Crystal _crystal;
        private Unit _unit;
        private int _crystalsCount;
        private int _unitsCount;

        [Inject]
        private void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;

            float delay = 0.5f;
            _checkDelay = new WaitForSeconds(delay);
        }

        public event Action<int> CrystalCollected = delegate { };

        public bool CanCollect =>
            TryGetUnit(out _) && _hasScannedCrystals;

        private bool _hasScannedCrystals =>
            _scannedCrystals.Count > 0;

        private void OnValidate()
        {
            if (_spawnPoint == null)
                throw new ArgumentNullException(nameof(_spawnPoint));
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out _crystal))
                AcceptCrystal(_crystal);
        }

        private void OnDisable()
        {
            if (_collectCrystalsCoroutine != null)
                StopCoroutine(_collectCrystalsCoroutine);
        }

        public void AddScannedCrystals(List<Crystal> crystals)
        {
            foreach (Crystal crystal in crystals)
                _scannedCrystals.Push(crystal);
        }

        public void CreateUnit()
        {
            _unit = _gameFactory.Create<Unit>(transform.position);
            _units.Add(_unit);
        }

        public void StartCollect()
        {
            _collectCrystalsCoroutine = StartCoroutine(CollectCrystalsAsync());
        }

        public void StopCollect()
        {
            if (_collectCrystalsCoroutine != null)
                StopCoroutine(_collectCrystalsCoroutine);
        }

        private IEnumerator CollectCrystalsAsync()
        {
            while (_hasScannedCrystals)
            {
                if (TryGetUnit(out _unit))
                {
                    _crystal = _scannedCrystals.Pop();
                    _unit.Collect(_crystal);
                }

                yield return _checkDelay;
            }
        }

        private bool TryGetUnit(out Unit standbyUnit)
        {
            foreach (Unit unit in _units)
            {
                if (unit.State.Equals(UnitStateTypes.Standby))
                {
                    standbyUnit = unit;
                    return true;
                }
            }

            standbyUnit = null;
            return false;
        }

        private void AcceptCrystal(Crystal crystal)
        {
            _crystalsCount++;
            CrystalCollected.Invoke(_crystalsCount);
            Destroy(crystal.gameObject);
        }
    }
}