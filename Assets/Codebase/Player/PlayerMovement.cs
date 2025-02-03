using Codebase.Services.Inputs;
using System;
using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public Action OnMoving;
        public Action OnJump;

        [SerializeField]
        private GroundChecker _groundChecker;

        [SerializeField]
        private Transform _transformMeshForRotate;

        [SerializeField]
        private float _moveSpeed = 5f;
        [SerializeField]
        private float _jumpForce = 10f;
        [SerializeField]
        private float _gravity = -9.81f;

        private CharacterController _characterController;
        private Vector3 _velocity;
        private Vector3 _inputMovement;

        [SerializeField]
        private DesktopInput _desktopInput;

        [field: SerializeField]
        public bool CanJump { get; private set; }
        [field: SerializeField]
        public bool IsGrounded { get; private set; }
        [field: SerializeField]
        public bool Idle { get; private set; }
        [field: SerializeField]
        public bool IsMoving { get; private set; }
        public bool IsDie { get; set; }

        public bool IsOnPlatform = false;
        private bool IsJumping = false; // Флаг для корректной работы прыжка с платформ

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (_groundChecker == null)
            {
                Debug.LogError("GroundChecker не назначен!");
            }
        }

        private void Update()
        {
            if (IsDie)
                return;

            // Игрок считается на земле, если либо GroundChecker, либо он находится на платформе
            IsGrounded = _groundChecker.IsGrounded || IsOnPlatform;

            _inputMovement = new Vector3(_desktopInput.Horizontal, 0f, 0f);

            if (_inputMovement != Vector3.zero)
            {
                HandleStates();
            }
            else
            {
                HandleIdle();
            }

            // Прыжок по нажатию клавиши
            if (_desktopInput.Jump)
            {
                PerformJump();
            }

            ApplyGravity();
        }

        private void LateUpdate()
        {
            _characterController.Move(_inputMovement * (_moveSpeed * Time.deltaTime));
            RotateTowardsDirection(ref _inputMovement);
        }

        private void HandleStates()
        {
            Idle = false;
            IsMoving = true;
            OnMoving?.Invoke();
        }

        private void HandleIdle()
        {
            Idle = true;
            IsMoving = false;
        }

        private void RotateTowardsDirection(ref Vector3 desiredDirection)
        {
            if (desiredDirection != Vector3.zero)
            {
                if (desiredDirection.x < 0 && _transformMeshForRotate.localRotation.y == 0)
                {
                    _transformMeshForRotate.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (desiredDirection.x > 0 && _transformMeshForRotate.localRotation.y != 0)
                {
                    _transformMeshForRotate.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
        }

        /// <summary>
        /// Выполняет прыжок.
        /// Если enemyBounce == true, прыжок выполняется независимо от состояния IsGrounded,
        /// что позволяет отскочить, даже если игрок стоит на враге.
        /// </summary>
        /// <param name="enemyBounce">Если true — прыжок при атаке врага.</param>
        public void PerformJump(bool enemyBounce = false)
        {
            // Если прыжок инициирован врагом, игнорируем проверку на IsGrounded
            if (IsGrounded || enemyBounce)
            {
                // Сбрасываем состояние платформы, чтобы гравитация не затирала начальную скорость прыжка
                IsOnPlatform = false;
                // Используем _jumpForce для расчёта начальной скорости прыжка
                _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
                CanJump = false;
                IsJumping = true;
                OnJump?.Invoke();
            }
        }

        private void ApplyGravity()
        {
            // Если игрок находится на платформе и не прыгает, не применять гравитацию
            if (IsOnPlatform && !IsJumping)
            {
                _velocity.y = 0f;
                return;
            }

            if (IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = _gravity + 5f;
                IsJumping = false;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (IsGrounded && !CanJump)
            {
                CanJump = true;
            }

            // Если игрок ударяется о потолок, отталкиваем его вниз
            if ((_characterController.collisionFlags & CollisionFlags.Above) != 0)
            {
                _velocity.y = -1f;
                _characterController.Move(new Vector3(0, -0.1f, 0));
            }
        }

        /// <summary>
        /// Мгновенно перемещает игрока в заданном направлении (например, для телепортации).
        /// </summary>
        /// <param name="direction">Направление и расстояние перемещения.</param>
        public void MoveForward(Vector3 direction)
        {
            _characterController.enabled = false;
            transform.position += direction;
            _characterController.enabled = true;
        }
    }
}
