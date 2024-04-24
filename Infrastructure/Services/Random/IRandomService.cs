using UnityEngine;

namespace Codebase.Infrastructure
{
    public interface IRandomService
    {
        Vector3 Range(Vector3 position1, Vector3 position2);
    }
}