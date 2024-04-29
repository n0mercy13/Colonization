using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Codebase.Logic;
using Codebase.StaticData;
using Codebase.View;
using System.Xml.Linq;

namespace Codebase.Infrastructure
{
    public partial class GameFactory
    {
        private readonly IObjectResolver _container;
        private readonly Unit _unitPrefab;
        private readonly Base _basePrefab;
        private readonly Crystal _crystalPrefab;
        private readonly ControlPanel _controlPanelPrefab;
        private readonly BuildMarker _buildMarkerPrefab;
        private readonly RectTransform _viewRoot;
        private readonly string _unitParentName = "Units";
        private readonly string _baseParentName = "Bases";
        private readonly string _crystalsParentName = "Resources";
        private Transform _unitParent;
        private Transform _baseParent;
        private Transform _crystalsParent;

        public GameFactory(IObjectResolver container, GameConfig gameConfig, SceneData sceneData)
        {
            _container = container;
            _unitPrefab = gameConfig.UnitPrefab;
            _basePrefab = gameConfig.BasePrefab;
            _crystalPrefab = gameConfig.CristalPrefab;
            _controlPanelPrefab = gameConfig.ControlPanelPrefab;
            _buildMarkerPrefab = gameConfig.BuildMarkerPrefab;
            _viewRoot = sceneData.ViewRoot;
        }
    }

    public partial class GameFactory : IGameFactory
    {
        public TObject Create<TObject>(Vector3 position) where TObject : MonoBehaviour
        {
            TObject prefab;
            Transform parent;

            if (typeof(TObject).Equals(typeof(Unit)))
            {
                if (_unitParent == null)
                    _unitParent = new GameObject(_unitParentName).transform;

                parent = _unitParent;
                prefab = _unitPrefab as TObject;
            }
            else if (typeof(TObject).Equals(typeof(Base)))
            {
                if (_baseParent == null)
                    _baseParent = new GameObject(_baseParentName).transform;

                parent = _baseParent;
                prefab = _basePrefab as TObject;
            }
            else if (typeof(TObject).Equals(typeof(Crystal)))
            {
                if (_crystalsParent == null)
                    _crystalsParent = new GameObject(_crystalsParentName).transform;

                parent = _crystalsParent;
                prefab = _crystalPrefab as TObject;
            }
            else if (typeof(TObject).Equals(typeof(BuildMarker)))
            {
                parent = _baseParent;
                prefab = _buildMarkerPrefab as TObject;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Cannot create unknown object of type: {typeof(TObject)}!");
            }

            return _container.Instantiate(prefab, position, Quaternion.identity, parent);
        }

        public TView CreateView<TView>() where TView : MonoBehaviour
        {
            TView prefab;

            if (typeof(TView).Equals(typeof(ControlPanel)))
            {
                prefab = _controlPanelPrefab as TView;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Cannot create unknown view of type: {typeof(TView)}!");
            }

            return _container.Instantiate(prefab, _viewRoot);
        }
    }
}
