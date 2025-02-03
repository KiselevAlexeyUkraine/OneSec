using UnityEngine;

namespace Codebase.Services.Inputs
{
    public class DesktopInput : MonoBehaviour, IInputService
    {
        public float Horizontal => Input.GetAxisRaw("Horizontal");
        public bool Jump => Input.GetKeyDown(KeyCode.Space);
        public bool Slide => Input.GetKeyDown(KeyCode.S);
        public bool Fire => Input.GetKeyDown(KeyCode.Mouse0);
        public bool Escape => Input.GetKeyDown(KeyCode.Escape);
    }
}
