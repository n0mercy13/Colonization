﻿using Codebase.Logic;
using Codebase.StaticData;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Codebase.Infrastructure
{
    public partial class GameFactory
    {
        private readonly IObjectResolver _container;
        private readonly Unit _unitPrefab;
        private readonly Base _basePrefab;
        private readonly Crystal _crystalPrefab;
        private readonly string _unitParentName = "Units";
        private readonly string _baseParentName = "Bases";
        private readonly string _crystalsParentName = "Resources";
        private Transform _unitParent;
        private Transform _baseParent;
        private Transform _crystalsParent;

        public GameFactory(IObjectResolver container, GameConfig gameConfig)
        {
            _container = container;
            _unitPrefab = gameConfig.UnitPrefab;
            _basePrefab = gameConfig.BasePrefab;
            _crystalPrefab = gameConfig.CristalPrefab;
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
            else
            {
                throw new InvalidOperationException(
                    $"Cannot create unknown object of type: {typeof(TObject)}!");
            }

            return _container.Instantiate(prefab, position, Quaternion.identity, parent);
        }
    }
}