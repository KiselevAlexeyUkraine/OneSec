using UnityEngine;

namespace Codebase.Player
{
    // Этот класс проверяет, находится ли объект на земле, используя заданную точку проверки.
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheck; // Трансформ, указывающий точку проверки на земле.
        [SerializeField] private LayerMask _groundMask; // Слой, определяющий, что считается землей.
        [SerializeField] private Vector3 _groundCheckSize = new Vector3(0.5f, 0.1f, 0.5f); // Размер квадратной области проверки.

        private void Awake()
        {
            if (_groundCheck == null)
            {
                Debug.LogError("GroundCheck Transform is not assigned!");
            }
        }

        // Свойство возвращает true, если объект касается слоя "земля".
        public bool IsGrounded => Physics.CheckBox(_groundCheck.position, _groundCheckSize / 2, Quaternion.identity, _groundMask);

        // Визуализирует область проверки в редакторе Unity.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue; // Устанавливает синий цвет для визуализации.
            Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize); // Рисует квадратную область проверки.
        }
    }
}
