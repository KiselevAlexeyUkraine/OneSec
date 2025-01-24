using UnityEngine;

namespace Codebase.Services.Inputs
{
    public class DesktopInput : IInputService
    {
        public bool Left => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        public bool Right => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        public bool Jump => Input.GetKeyDown(KeyCode.Space);
        public bool Slide => Input.GetKeyDown(KeyCode.S);
        public bool Fire => Input.GetKeyDown(KeyCode.Mouse0);
        public bool Escape => Input.GetKeyDown(KeyCode.Escape);
    }
}
