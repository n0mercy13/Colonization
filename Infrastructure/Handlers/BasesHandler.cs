using System;
using System.Collections.Generic;
using UnityEngine;
using Codebase.Logic;
using Codebase.StaticData;

namespace Codebase.Infrastructure
{
    public partial class BasesHandler
    {
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _gameInputService;
        private readonly IRaycastService _raycastService;
        private readonly int _initialUnitsOnBase;
        private readonly Vector3 _initialPosition;
        private readonly List<Crystal> _crystals;

        public BasesHandler(
            IGameFactory gameFactory, 
            IInputService gameInputService, 
            IRaycastService raycastService, 
            GameConfig gameConfig, 
            SceneData sceneData)
        {
            _gameFactory = gameFactory;
            _gameInputService = gameInputService;
            _raycastService = raycastService;
            _initialUnitsOnBase = gameConfig.InitialUnitsOnBase;
            _initialPosition = sceneData.InitialBaseLocation.position;
            _crystals = new List<Crystal>(gameConfig.InitialCrystals);

            _gameInputService.Selected += OnSelected;
        }

        private void OnSelected(Vector2 mouseScreenPosition)
        {
            if(_raycastService.TryScanCrystals(
                mouseScreenPosition, out List<Crystal> crystals))
                    _crystals.AddRange(crystals);
        }
    }

    public partial class BasesHandler : IInitializable
    {
        public void Initialize()
        {
            Base @base = _gameFactory.Create<Base>(_initialPosition);
            
            for(int i = 0; i < _initialUnitsOnBase; i++)
            {
                Unit unit = _gameFactory.Create<Unit>(@base.transform.position);
            }
        }
    }

    public partial class BasesHandler : IDisposable
    {
        public void Dispose()
        {
            _gameInputService.Selected -= OnSelected;
        }
    }
}
