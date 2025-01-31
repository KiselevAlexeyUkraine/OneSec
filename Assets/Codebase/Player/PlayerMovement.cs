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
        private bool IsJumping = false; // Новый флаг для корректной работы прыжка с платформ

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (_groundChecker == null)
            {
                Debug.LogError("GroundChecker is not assigned!");
            }
        }

        private void Update()
        {
            if (IsDie == true)
                return;
            
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

        private void PerformJump()
        {
            if (IsGrounded) // Теперь можно прыгать с платформы
            {
                _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
                CanJump = false;
                IsJumping = true; // Фиксируем, что игрок прыгнул
                OnJump?.Invoke();
            }
        }

        private void ApplyGravity()
        {
            if (IsOnPlatform && !IsJumping)
            {
                _velocity.y = 0f; // Отключаем гравитацию, если стоим на платформе и не прыгаем
                return;
            }

            if (IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = _gravity + 5f;
                IsJumping = false; // Сбрасываем флаг прыжка при приземлении
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (IsGrounded && !CanJump)
            {
                CanJump = true;
            }

            // Если игрок ударяется о платформу головой, мгновенно отталкиваем вниз
            if ((_characterController.collisionFlags & CollisionFlags.Above) != 0)
            {
                _velocity.y = -1f; // Принудительно отталкиваем вниз
                _characterController.Move(new Vector3(0, -0.1f, 0)); // Двигаем вниз, чтобы отлип
            }
        }

        /// <summary>
        /// Телепортирует игрока или перемещает его мгновенно в указанное направление, даже если он находится в прыжке.
        /// </summary>
        /// <param name="direction">Направление и расстояние для перемещения.</param>
        public void MoveForward(Vector3 direction)
        {
            _characterController.enabled = false; // Выключаем контроллер для корректного телепорта
            transform.position += direction; // Мгновенно перемещаем игрока
            _characterController.enabled = true; // Включаем контроллер обратно
        }
    }
}
