using Codebase.Services.Inputs;
using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerMovementPhysics : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f; // Скорость передвижения
        [SerializeField] private float jumpForce = 10f; // Сила прыжка

        [SerializeField]
        private GroundChecker _groundChecker;

        [SerializeField] 
        private Transform _transformMeshForRotate;

        private DesktopInput _desktopInput = new();

        private Rigidbody _rigidbody;
        private bool _isGrounded;

        private Vector3 _movementInput;
        private bool _jumpRequested;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_groundChecker == null)
            {
                Debug.LogError("GroundChecker is not assigned!");
            }
        }

        private void Update()
        {
            // Получаем ввод пользователя только по горизонтальной оси
            _movementInput = new Vector3(_desktopInput.Horizontal, 0f, 0f);
            _movementInput = _movementInput.normalized;

            _isGrounded = _groundChecker.IsGrounded;

            if (_desktopInput.Jump && _isGrounded)
            {
                _jumpRequested = true;
            }

            HandleRotation();
        }

        private void FixedUpdate()
        {
            MovePlayer();

            if (_jumpRequested)
            {
                PerformJump();
                _jumpRequested = false;
            }
        }

        private void MovePlayer()
        {
            // Преобразуем движение относительно камеры
            Vector3 moveDirection = transform.TransformDirection(_movementInput) * moveSpeed;
            Vector3 velocity = new Vector3(moveDirection.x, _rigidbody.velocity.y, _rigidbody.velocity.z);

            // Применяем скорость к Rigidbody
            _rigidbody.velocity = velocity;
        }

        private void PerformJump()
        {
            if (_isGrounded)
            {
                // Применяем силу прыжка
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void HandleRotation()
        {
            // Поворот на 180 градусов при нажатии клавиш A и D
            if (_desktopInput.Horizontal < 0) // Нажата клавиша A
            {
                _transformMeshForRotate.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (_desktopInput.Horizontal > 0) // Нажата клавиша D
            {
                _transformMeshForRotate.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
}
