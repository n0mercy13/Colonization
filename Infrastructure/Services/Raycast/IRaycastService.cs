using Codebase.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Infrastructure
{
    public interface IRaycastService
    {
        bool TryScanCrystals(Vector2 mouseScreenPosition, out List<Crystal> crystals);
    }
}