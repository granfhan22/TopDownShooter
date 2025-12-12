using UnityEngine;
using UnityEngine.InputSystem;

namespace Topdown.movement
{
    public class GunRotation : Rotator
    {
        
        private void Update()
        {
            Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            LookAt(MousePosition);
        }

    }
}
