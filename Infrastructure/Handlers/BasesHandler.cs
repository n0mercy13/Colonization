using System;
using System.Collections.Generic;
using UnityEngine;
using Codebase.Logic;
using Codebase.StaticData;
using Codebase.View;

namespace Codebase.Infrastructure
{
    public partial class BasesHandler
    {
        private readonly IGameFactory _gameFactory;
        private readonly IRaycastService _raycastService;
        private readonly Vector3 _initialPosition;
        private ControlPanel _controlPanel;
        private Base _activeBase;

        public BasesHandler(
            IGameFactory gameFactory, 
            IRaycastService raycastService, 
            SceneData sceneData)
        {
            _gameFactory = gameFactory;
            _raycastService = raycastService;
            _initialPosition = sceneData.InitialBaseLocation.position;

            _raycastService.BaseSelected += OnBaseSelected;
            _raycastService.BuildPositionSelected += OnBuildPositionSelected;
            _raycastService.CrystalsScanned += OnCrystalsScanned;
        }

        private void OnCrystalsScanned(List<Crystal> crystals) => 
            _activeBase.AddScannedCrystals(crystals);

        private void OnBuildPositionSelected(Vector3 position) => 
            _activeBase.PlaceBuildMarker(position);

        private void OnBaseSelected(Base @base)
        {
            if(_activeBase != null)
                _activeBase.Unselected();

            _activeBase = @base;
            _controlPanel.Register(_activeBase);
            _activeBase.Selected();
        }
    }

    public partial class BasesHandler : IInitialize
    {
        public void Initialize()
        {
            _controlPanel = _gameFactory.CreateView<ControlPanel>();
            _activeBase = _gameFactory.Create<Base>(_initialPosition);

            if(_activeBase is IInitialize initializable)
                initializable.Initialize();

            _controlPanel.Register(_activeBase);
            _activeBase.Selected();
        }
    }

    public partial class BasesHandler : IDisposable
    {
        public void Dispose()
        {
            _raycastService.BaseSelected -= OnBaseSelected;
            _raycastService.BuildPositionSelected -= OnBuildPositionSelected;
            _raycastService.CrystalsScanned -= OnCrystalsScanned;
        }
    }
}
