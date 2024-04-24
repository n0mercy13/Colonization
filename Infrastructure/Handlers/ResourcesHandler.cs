using System.Collections.Generic;
using UnityEngine;
using Codebase.Logic;
using Codebase.StaticData;

namespace Codebase.Infrastructure
{
    public partial class ResourcesHandler
    {
        private readonly IGameFactory _gameFactory;
        private readonly IRandomService _randomService;
        private readonly List<Crystal> _crystals;
        private readonly Vector3 _boundaryPoint1;
        private readonly Vector3 _boundaryPoint2;
        private readonly int _initialCrystals;

        public ResourcesHandler(IGameFactory gameFactory, IRandomService randomService, GameConfig gameConfig, SceneData sceneData)
        {
            _gameFactory = gameFactory;
            _randomService = randomService;
            _crystals = new List<Crystal>(_initialCrystals);
            _initialCrystals = gameConfig.InitialCrystals;
            _boundaryPoint1 = sceneData.Ground.BoundaryPoint1;
            _boundaryPoint2 = sceneData.Ground.BoundaryPoint2;
        }
    }

    public partial class ResourcesHandler : IInitializable
    {
        public void Initialize()
        {
            Crystal crystal;
            Vector3 spawnPosition;

            for(int i = 0; i < _initialCrystals; i++)
            {
                spawnPosition = _randomService.Range(_boundaryPoint1, _boundaryPoint2);
                crystal = _gameFactory.Create<Crystal>(spawnPosition);
                _crystals.Add(crystal);
            }
        }
    }
}
