using System;
using UnityEngine;
using Codebase.Logic;
using Codebase.View;

namespace Codebase.StaticData
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "StaticData/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public Unit UnitPrefab { get; private set; }
        [field: SerializeField] public Base BasePrefab { get; private set; }
        [field: SerializeField] public Crystal CristalPrefab { get; private set; }
        [field: SerializeField] public ControlPanel ControlPanelPrefab { get; private set; }
        [field: SerializeField] public BuildMarker BuildMarkerPrefab { get; private set; }
        [field: SerializeField, Range(1, 100)] public int InitialCrystals { get; private set; } = 50;
        [field: SerializeField, Range(1, 10)] public int InitialUnitsOnBase { get; private set; } = 3;
        [field: SerializeField, Range(0f, 20f)] public float ScanRadius { get; private set; } = 5.0f;
        [field: SerializeField, Range(1, 5)] public int UnitCost { get; private set; } = 3;
        [field: SerializeField, Range(1, 10)] public int BaseCost { get; private set; } = 5;
        [field: SerializeField] public LayerMask GroundMask { get; private set; }
        [field: SerializeField] public LayerMask CrystalMask { get; private set; }
        [field: SerializeField] public LayerMask BaseMask { get; private set; }
        [field: SerializeField] public Color CrystalScannedColor { get; private set; }

        private void OnValidate()
        {
            if (UnitPrefab == null)
                throw new ArgumentNullException(nameof(UnitPrefab));

            if (BasePrefab == null)
                throw new ArgumentNullException(nameof(BasePrefab));

            if (CristalPrefab == null)
                throw new ArgumentNullException(nameof(CristalPrefab));

            if (ControlPanelPrefab == null)
                throw new ArgumentNullException(nameof(ControlPanelPrefab));

            if(BuildMarkerPrefab == null)
                throw new ArgumentNullException(nameof(BuildMarkerPrefab));
        }
    }
}
