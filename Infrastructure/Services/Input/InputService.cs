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
        }

        private Vector2 _mouseScreenPosition =>
            Mouse.current.position.ReadValue();

        private void OnSelectPerformed(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
                Selected.Invoke(_mouseScreenPosition);
        }
    }

    public partial class InputService : IInputService
    {
        public event Action<Vector2> Selected = delegate { };
    }

    public partial class InputService : IDisposable
    {
        public void Dispose()
        {
            _inputs.Gameplay.Select.performed -= OnSelectPerformed;
        }
    }
}
