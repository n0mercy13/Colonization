using Codebase.Logic;
using Codebase.StaticData;
using UnityEngine;

namespace Codebase.Infrastructure
{
    public partial class BasesHandler
    {
        private readonly IGameFactory _gameFactory;
        private readonly int _initialUnitsOnBase;
        private readonly Vector3 _initialPosition;

        public BasesHandler(IGameFactory gameFactory, GameConfig gameConfig, SceneData sceneData)
        {
            _gameFactory = gameFactory;
            _initialUnitsOnBase = gameConfig.InitialUnitsOnBase;
            _initialPosition = sceneData.InitialBaseLocation.position;
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
}
