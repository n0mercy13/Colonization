using System;
using System.Collections.Generic;
using UnityEngine;
using Codebase.Logic;
using Codebase.StaticData;

namespace Codebase.Infrastructure
{
    public partial class RaycastService
    {
        private readonly IInputService _inputService;
        private readonly Camera _camera;
        private readonly Collider[] _results;
        private readonly RaycastHit[] _raycastHits;
        private readonly LayerMask _baseMask;
        private readonly LayerMask _groundMask;
        private readonly LayerMask _crystalMask;
        private readonly float _scanRadius;
        private readonly float _maxDistance;
        private Crystal _crystal;
        private Ray _ray;
        private int _hits;

        public RaycastService(IInputService inputService, SceneData sceneData, GameConfig gameConfig)
        {
            _inputService = inputService;
            _camera = sceneData.Camera;
            _baseMask = gameConfig.BaseMask;
            _groundMask = gameConfig.GroundMask;
            _crystalMask = gameConfig.CrystalMask;
            _scanRadius = gameConfig.ScanRadius;

            _maxDistance = 200.0f;
            int maxScannedCrystals = 20;
            _results = new Collider[maxScannedCrystals];
            _raycastHits = new RaycastHit[1];

            _inputService.SelectBasePressed += OnSelectBasePressed;
            _inputService.ScanPressed += OnScanPressed;
            _inputService.BuildPressed += OnBuildPressed;
        }

        private void OnScanPressed(Vector2 mouseScreenPosition)
        {
            MakeRaycast(mouseScreenPosition, _groundMask);

            if(_hits > 0 
                && _raycastHits[0].collider.TryGetComponent<Ground>(out _)
                && TryScanCrystals(_raycastHits[0].point, out List<Crystal> crystals))
            {
                CrystalsScanned.Invoke(crystals);
            }
        }

        private void OnSelectBasePressed(Vector2 mouseScreenPosition)
        {
            MakeRaycast(mouseScreenPosition, _baseMask);

            if (_hits > 0 && _raycastHits[0].collider.TryGetComponent(out Base @base))
                BaseSelected.Invoke(@base);
        }

        private void OnBuildPressed(Vector2 mouseScreenPosition)
        {
            MakeRaycast(mouseScreenPosition, _groundMask);

            if (_hits > 0 && _raycastHits[0].collider.TryGetComponent(out Ground ground))
                BuildPositionSelected.Invoke(_raycastHits[0].point);
        }

        private void MakeRaycast(Vector2 mouseScreenPosition, LayerMask mask)
        {
            _ray = _camera.ScreenPointToRay(mouseScreenPosition);
            _hits = Physics.RaycastNonAlloc(
                _ray, _raycastHits, _maxDistance, mask, QueryTriggerInteraction.Collide);
        }

        private bool TryScanCrystals(Vector3 position, out List<Crystal> crystals)
        {
            crystals = new List<Crystal>();

            _hits = Physics.OverlapSphereNonAlloc(
                position, _scanRadius, _results, _crystalMask);

            if (_hits <= 0)
                return false;

            for (int i = 1; i < _hits; i++)
            {
                if (_results[i].TryGetComponent(out _crystal)
                    && _crystal.IsScanned == false)
                {
                    _crystal.SetScanned();
                    crystals.Add(_crystal);
                }
            }

            return true;
        }
    }

    public partial class RaycastService : IRaycastService
    {
        public event Action<Base> BaseSelected = delegate { };
        public event Action<Vector3> BuildPositionSelected = delegate { };
        public event Action<List<Crystal>> CrystalsScanned = delegate { };
    }

    public partial class RaycastService : IDisposable
    {
        public void Dispose()
        {
            _inputService.SelectBasePressed -= OnSelectBasePressed;
            _inputService.ScanPressed -= OnScanPressed;
            _inputService.BuildPressed -= OnBuildPressed;
        }
    }
}
