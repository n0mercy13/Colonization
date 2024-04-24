using System;
using UnityEngine;

namespace Codebase.Logic
{
    public class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _boundaryPoint1;
        [SerializeField] private Transform _boundaryPoint2;

        private void OnValidate()
        {
            if(_boundaryPoint1 == null)
                throw new ArgumentNullException(nameof(_boundaryPoint1));

            if( _boundaryPoint2 == null)
                throw new ArgumentNullException(nameof(_boundaryPoint2));
        }

        public Vector3 BoundaryPoint1 => _boundaryPoint1.position;
        public Vector3 BoundaryPoint2 => _boundaryPoint2.position;
    }
}
