using System;
using UnityEngine;
using Codebase.Logic;

namespace Codebase.StaticData
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "StaticData/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public Unit UnitPrefab { get; private set; }
        [field: SerializeField] public Base BasePrefab { get; private set; }
        [field: SerializeField] public Crystal CristalPrefab { get; private set; }
        [field: SerializeField, Range(1, 100)] public int InitialCrystals { get; private set; } = 50;
        [field: SerializeField, Range(1, 10)] public int InitialUnitsOnBase { get; private set; } = 3;
        [field: SerializeField, Range(0f, 20f)] public float ScanRadius { get; private set; } = 5.0f;
        [field: SerializeField] public LayerMask GroundMask { get; private set; }
        [field: SerializeField] public LayerMask CrystalMask { get; private set; }
        [field: SerializeField] public Color CrystalScannedColor { get; private set; }

        private void OnValidate()
        {
            if(UnitPrefab == null)
                throw new ArgumentNullException(nameof(UnitPrefab));

            if(BasePrefab == null)
                throw new ArgumentNullException(nameof(BasePrefab));

            if(CristalPrefab == null)
                throw new ArgumentNullException(nameof(CristalPrefab));
        }
    }
}
