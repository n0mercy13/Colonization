using System;
using UnityEngine;

namespace Codebase.Infrastructure
{
    public interface IInputService
    {
        event Action<Vector2> Selected;
    }
}