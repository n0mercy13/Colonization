using System.Collections.Generic;
using UnityEngine;
using Codebase.Logic;
using Codebase.StaticData;

namespace Codebase.Infrastructure
{
    public partial class RaycastService
    {
        private readonly Camera _camera;
        private readonly Collider[] _results;
        private readonly RaycastHit[] _raycastHits;
        private readonly LayerMask _groundMask;
        private readonly LayerMask _crystalMask;
        private readonly float _scanRadius;
        private readonly float _maxDistance;
        private Crystal _crystal;
        private Ray _ray;
        private int _hits;

        public RaycastService(SceneData sceneData, GameConfig gameConfig)
        {
            _camera = sceneData.Camera;
            _groundMask = gameConfig.GroundMask;
            _crystalMask = gameConfig.CrystalMask;
            _scanRadius = gameConfig.ScanRadius;
            _maxDistance = 200.0f;
            int maxScannedCrystals = 20;
            _results = new Collider[maxScannedCrystals];
            _raycastHits = new RaycastHit[1];
        }
    }

    public partial class RaycastService : IRaycastService
    {
        public bool TryScanCrystals(Vector2 mouseScreenPosition, out List<Crystal> crystals)
        {
            crystals = new List<Crystal>();

            _ray = _camera.ScreenPointToRay(mouseScreenPosition);
            _hits = Physics.RaycastNonAlloc(_ray, _raycastHits, _maxDistance, _groundMask);

            if(_hits <= 0)
                return false;

            _hits = Physics.OverlapSphereNonAlloc(
                _raycastHits[0].point, _scanRadius, _results, _crystalMask);

            if(_hits <= 0) 
                return false;

            for(int i = 1; i < _hits; i++)
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
}
