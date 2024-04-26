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
        private readonly IInputService _gameInputService;
        private readonly IRaycastService _raycastService;
        private readonly Vector3 _initialPosition;
        private readonly int _initialUnitsOnBase;
        private ControlPanel _controlPanel;
        private Base _activeBase;

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

            _gameInputService.Selected += OnSelected;
        }

        private void OnSelected(Vector2 mouseScreenPosition)
        {
            if(_raycastService.TryScanCrystals(
                mouseScreenPosition, out List<Crystal> crystals)
                && _controlPanel.IsScanActive)
            {
                foreach (Crystal crystal in crystals)
                    crystal.SetScanned();

                _activeBase.AddScannedCrystals(crystals);
            }
        }

        private void OnCollectPressed(bool isCollectActive)
        {
            if (isCollectActive && _activeBase.CanCollect)
                _activeBase.StartCollect();
            else if (isCollectActive == false)
                _activeBase.StopCollect();
            else if(isCollectActive && _activeBase.CanCollect == false)
                _controlPanel.StopCollectAnimation();
        }

        private void OnCrystalCollected(int crystalsCount) => 
            _controlPanel.UpdateCrystalInfo(crystalsCount);
    }

    public partial class BasesHandler : IInitializable
    {
        public void Initialize()
        {
            Base @base = _gameFactory.Create<Base>(_initialPosition);

            for (int i = 0; i < _initialUnitsOnBase; i++)
                @base.CreateUnit();

            _activeBase = @base;
            _activeBase.CrystalCollected += OnCrystalCollected;

            _controlPanel = _gameFactory.CreateView<ControlPanel>();
            _controlPanel.CollectPressed += OnCollectPressed;
        }
    }

    public partial class BasesHandler : IDisposable
    {
        public void Dispose()
        {
            _gameInputService.Selected -= OnSelected;
            _activeBase.CrystalCollected -= OnCrystalCollected;

            if(_controlPanel != null)
                _controlPanel.CollectPressed -= OnCollectPressed;
        }
    }
}
