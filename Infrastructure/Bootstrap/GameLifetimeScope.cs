using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Codebase.StaticData;
using Codebase.Infrastructure;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private SceneData _sceneData;

    private void OnValidate()
    {
        if(_gameConfig == null)
            throw new ArgumentNullException(nameof(_gameConfig));

        if(_sceneData == null)
            throw new ArgumentNullException(nameof(_sceneData));
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder
            .RegisterInstance(_gameConfig);
        builder
            .RegisterInstance(_sceneData);
        builder
            .RegisterEntryPoint<Bootstrap>(Lifetime.Singleton)
            .AsSelf();
        builder
            .Register<GameFactory>(Lifetime.Singleton)
            .AsImplementedInterfaces();
        builder
            .Register<ResourcesHandler>(Lifetime.Singleton)
            .AsImplementedInterfaces();
        builder
            .Register<BasesHandler>(Lifetime.Singleton)
            .AsImplementedInterfaces();
        builder
            .Register<RandomService>(Lifetime.Singleton)
            .AsImplementedInterfaces();
        builder
            .Register<InputActions>(Lifetime.Singleton)
            .AsSelf();
        builder
            .Register<InputService>(Lifetime.Singleton)
            .AsImplementedInterfaces();
        builder
            .Register<RaycastService>(Lifetime.Singleton)
            .AsImplementedInterfaces();
    }
}
