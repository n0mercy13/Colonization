using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Codebase.Infrastructure;
using Codebase.StaticData;

namespace Codebase.Logic
{
    [RequireComponent(typeof(Collider))]
    public partial class Base : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        private readonly Stack<Crystal> _scannedCrystals = new();
        private readonly List<Unit> _units = new();
        private IGameFactory _gameFactory;
        private BuildMarker _buildMarker;
        private Coroutine _collectCrystalsCoroutine;
        private Coroutine _createUnitsCoroutine;
        private Coroutine _buildNewBaseCoroutine;
        private YieldInstruction _checkDelay;
        private Crystal _crystal;
        private Unit _unit;
        private int _crystalsCount;
        private int _initialUnitsCount;
        private int _unitCost;
        private int _baseCost;

        [Inject]
        private void Construct(IGameFactory gameFactory, GameConfig gameConfig)
        {
            _gameFactory = gameFactory;
            _initialUnitsCount = gameConfig.InitialUnitsOnBase;
            _unitCost = gameConfig.UnitCost;
            _baseCost = gameConfig.BaseCost;

            float delay = 0.5f;
            _checkDelay = new WaitForSeconds(delay);
        }

        public event Action<int> CrystalsCountChanged = delegate { };
        public event Action<int> UnitsCountChanged = delegate { };

        private bool _hasScannedCrystals =>
            _scannedCrystals.Count > 0;

        private void OnValidate()
        {
            if (_spawnPoint == null)
                throw new ArgumentNullException(nameof(_spawnPoint));
        }

        private void Start()
        {
            _collectCrystalsCoroutine = StartCoroutine(CollectCrystalsAsync());
            _createUnitsCoroutine = StartCoroutine(CreateUnitsAsync());
        }

        private void OnDisable()
        {
            if (_collectCrystalsCoroutine != null)
                StopCoroutine(_collectCrystalsCoroutine);

            if(_createUnitsCoroutine != null)
                StopCoroutine(_createUnitsCoroutine);

            if(_buildNewBaseCoroutine != null)
                StopCoroutine(_buildNewBaseCoroutine);

            foreach (Unit unit in _units)
                UnRegister(unit);
        }

        public void Selected()
        {
            RefreshInfo();
            
            if(_buildMarker != null)
                _buildMarker.UnHide();
        }

        public void Unselected()
        {
            if(_buildMarker != null)
                _buildMarker.Hide();
        }

        public void AddScannedCrystals(List<Crystal> crystals)
        {
            foreach (Crystal crystal in crystals)
                _scannedCrystals.Push(crystal);
        }

        public void PlaceBuildMarker(Vector3 position)
        {
            if (_buildMarker != null)
            {
                _buildMarker.transform.position = position;
                return;
            }

            _buildMarker = _gameFactory.Create<BuildMarker>(position);

            if(_createUnitsCoroutine != null)
                StopCoroutine(_createUnitsCoroutine);

            _buildNewBaseCoroutine = StartCoroutine(BuildNewBaseAsync());
        }

        public void Register(Unit unit)
        {
            _units.Add(unit);
            unit.CrystalDelivered += OnCrystalDelivered;
        }

        private void UnRegister(Unit unit)
        {
            _units.Remove(unit);
            unit.CrystalDelivered -= OnCrystalDelivered;
        }

        private void OnCrystalDelivered()
        {
            _crystalsCount++;
            CrystalsCountChanged.Invoke(_crystalsCount);
        }

        private void RefreshInfo()
        {
            CrystalsCountChanged.Invoke(_crystalsCount);
            UnitsCountChanged.Invoke(_units.Count);
        }

        private void CreateUnit()
        {
            _unit = _gameFactory
                .Create<Unit>(transform.position);
            Register(_unit);
            UnitsCountChanged.Invoke(_units.Count);
        }

        private IEnumerator CollectCrystalsAsync()
        {
            while (enabled)
            {
                if (TryGetUnit(out _unit)
                    && _hasScannedCrystals)
                {
                    _crystal = _scannedCrystals.Pop();
                    _unit.Collect(_crystal);
                }

                yield return _checkDelay;
            }
        }

        private IEnumerator CreateUnitsAsync()
        {
            while(enabled)
            {
                if(_crystalsCount >= _unitCost)
                {
                    _crystalsCount -= _unitCost;
                    CrystalsCountChanged.Invoke(_crystalsCount);

                    CreateUnit();
                }

                yield return _checkDelay;
            }
        }

        private IEnumerator BuildNewBaseAsync()
        {
            while(_buildMarker != null)
            {
                if(_crystalsCount >= _baseCost
                    && TryGetUnit(out _unit))
                {
                    _crystalsCount -= _baseCost;
                    _unit.BuildBase(_buildMarker.transform.position, _gameFactory.Create<Base>);
                    UnRegister(_unit);
                    UnitsCountChanged.Invoke(_units.Count);
                    Destroy(_buildMarker.gameObject);
                }

                yield return _checkDelay;
            }

            _collectCrystalsCoroutine = StartCoroutine(CollectCrystalsAsync());
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
    }

    public partial class Base : IInitialize
    {
        public void Initialize()
        {
            for (int i = 0; i < _initialUnitsCount; i++)
                CreateUnit();
        }
    }
}