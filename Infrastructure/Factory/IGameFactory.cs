using UnityEngine;

namespace Codebase.Infrastructure
{
    public interface IGameFactory
    {
        TObject Create<TObject>(Vector3 position) where TObject : MonoBehaviour;
        TView CreateView<TView>() where TView : MonoBehaviour;
    }
}