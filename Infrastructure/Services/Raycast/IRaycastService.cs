using System;
using System.Collections.Generic;
using UnityEngine;
using Codebase.Logic;

namespace Codebase.Infrastructure
{
    public interface IRaycastService
    {
        event Action<Base> BaseSelected;
        event Action<Vector3> BuildPositionSelected;
        event Action<List<Crystal>> CrystalsScanned;
    }
}