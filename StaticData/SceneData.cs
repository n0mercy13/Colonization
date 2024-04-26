using Codebase.Logic;
using System;
using UnityEngine;

namespace Codebase.StaticData
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Ground Ground { get; private set; }
        [field: SerializeField] public RectTransform ViewRoot { get; private set; }
        [field: SerializeField] public Transform InitialBaseLocation { get; private set; }

        private void OnValidate()
        {
            if (Camera == null)
                throw new ArgumentNullException(nameof(Camera));

            if (Ground == null)
                throw new ArgumentNullException(nameof(Ground));

            if(ViewRoot == null)
                throw new ArgumentNullException(nameof(ViewRoot));

            if (InitialBaseLocation == null)
                throw new ArgumentNullException(nameof(InitialBaseLocation));
        }
    }
}
