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
        private float _rotationSpeed = 10f;
        [SerializeField]
        private float _jumpForce = 10f;
        [SerializeField]
        private float _gravity = -9.81f;

        private CharacterController _characterController;
        private Vector3 _velocity;
        private Vector3 _inputMovement;
        private DesktopInput _desktopInput = new();

        [field: SerializeField]
        public bool CanJump { get; private set; }
        [field: SerializeField]
        public bool IsGrounded { get; private set; }
        [field: SerializeField]
        public bool Idle { get; private set; }
        [field: SerializeField]
        public bool IsMoving { get; private set; }

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
            IsGrounded = _groundChecker.IsGrounded;

            _inputMovement = new Vector3(_desktopInput.Horizontal, 0f, 0f);

            if (_inputMovement != Vector3.zero)
            {
                HandleStates();
            }
            else
            {
                HandleIdle();
            }

            if (_desktopInput.Jump && IsGrounded && CanJump)
            {
                PerformJump();
            }

            ApplyGravity();
        }

        private void LateUpdate()
        {
            _characterController.Move(_inputMovement * (_moveSpeed * Time.deltaTime));

            RotateTowardsDirection(_inputMovement);
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

        private void RotateTowardsDirection(Vector3 desiredDirection)
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
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
            CanJump = false;
            OnJump?.Invoke();
        }

        private void ApplyGravity()
        {
            if (IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = -1f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (IsGrounded && !CanJump)
            {
                CanJump = true;
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
