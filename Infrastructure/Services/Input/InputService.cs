using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Codebase.Infrastructure
{
    public partial class InputService
    {
        private readonly InputActions _inputs;

        public InputService(InputActions inputs)
        {
            _inputs = inputs;
            _inputs.Enable();

            _inputs.Gameplay.Select.performed += OnSelectPerformed;
            _inputs.Gameplay.Scan.performed += OnScanPerformed;
            _inputs.Gameplay.Build.performed += OnBuildPerformed;
        }

        private Vector2 _mouseScreenPosition =>
            Mouse.current.position.ReadValue();

        private void OnSelectPerformed(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
                SelectBasePressed.Invoke(_mouseScreenPosition);
        }

        private void OnScanPerformed(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
                ScanPressed.Invoke(_mouseScreenPosition);
        }

        private void OnBuildPerformed(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
                BuildPressed.Invoke(_mouseScreenPosition);
        }
    }

    public partial class InputService : IInputService
    {
        public event Action<Vector2> SelectBasePressed = delegate { };
        public event Action<Vector2> ScanPressed = delegate { };
        public event Action<Vector2> BuildPressed = delegate { };
    }

    public partial class InputService : IDisposable
    {
        public void Dispose()
        {
            _inputs.Gameplay.Select.performed -= OnSelectPerformed;
            _inputs.Gameplay.Scan.performed -= OnScanPerformed;
            _inputs.Gameplay.Build.performed -= OnBuildPerformed;
        }
    }
}
