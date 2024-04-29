using System;
using TMPro;
using UnityEngine;
using Codebase.Logic;
using Codebase.Infrastructure;

namespace Codebase.View
{
    public partial class ControlPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _unitsLabel;
        [SerializeField] private TMP_Text _crystalsLabel;

        private Base _currentBase;

        private void OnValidate()
        {
            if (_unitsLabel == null)
                throw new ArgumentNullException(nameof(_unitsLabel));

            if (_crystalsLabel == null)
                throw new ArgumentNullException(nameof(_crystalsLabel));
        }

        private void OnDestroy()
        {
            _currentBase.UnitsCountChanged -= OnUnitsCountChanged;
            _currentBase.CrystalsCountChanged -= OnCrystalCountChanged;
        }

        private void OnUnitsCountChanged(int unitsCount) =>
            _unitsLabel.text = $"Units : {unitsCount:D2}";

        private void OnCrystalCountChanged(int crystalsCount) =>
            _crystalsLabel.text = $"Crystals : {crystalsCount:D3}";
    }

    public partial class ControlPanel : IRegister<Base>
    {
        public void Register(Base currentBase)
        {
            if(_currentBase != null)
            {
                _currentBase.UnitsCountChanged -= OnUnitsCountChanged;
                _currentBase.CrystalsCountChanged -= OnCrystalCountChanged;
            }

            _currentBase = currentBase;

            _currentBase.UnitsCountChanged += OnUnitsCountChanged;
            _currentBase.CrystalsCountChanged += OnCrystalCountChanged;
        }
    }
}
