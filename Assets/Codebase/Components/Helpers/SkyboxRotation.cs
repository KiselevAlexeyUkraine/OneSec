using UnityEngine;

namespace Codebase.Components.Helpers
{
    public class SkyboxRotation : MonoBehaviour
    {
        public Material skybox;
        public float angleSpeed = 1f;
        public Gradient gradient;

        private float _angle;
        
        private static readonly int Tint = Shader.PropertyToID("_Tint");
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");

        private void Update()
        {
            _angle += angleSpeed * Time.deltaTime;

            if (_angle > 360f)
            {
                _angle = 0f;
            }
            
            skybox.SetColor(Tint, gradient.Evaluate(_angle / 360f));
            skybox.SetFloat(Rotation, _angle);
        }
    }
}
