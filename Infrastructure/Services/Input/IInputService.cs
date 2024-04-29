using System;
using UnityEngine;

namespace Codebase.Infrastructure
{
    public interface IInputService
    {
        event Action<Vector2> SelectBasePressed;
        event Action<Vector2> ScanPressed;
        event Action<Vector2> BuildPressed;
    }
}