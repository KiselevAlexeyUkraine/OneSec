using Codebase.Services.Inputs;
using System;
using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public Action OnMoving;
        public Action OnJumpStarted; // Событие для звука прыжка
        public Action OnJump; // Событие фактического прыжка
        public Action OnStartRunning;
        public Action OnStopRunning;
        public Action OnStartIdle;

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
        public bool JustJumped { get; private set; }
        [field: SerializeField]
        public bool IsGrounded { get; private set; }
        [field: SerializeField]
        public bool IsIdle { get; private set; }
        [field: SerializeField]
        public bool IsMoving { get; private set; }
        public bool IsDie { get; set; }

        public bool IsOnPlatform = false;
        public bool IsJumping { get; private set; }

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

            IsGrounded = _groundChecker.IsGrounded || IsOnPlatform;

            _inputMovement = new Vector3(_desktopInput.Horizontal, 0f, 0f);

            if (_inputMovement != Vector3.zero)
            {
                if (!IsMoving)
                {
                    OnStartRunning?.Invoke();
                }
                HandleStates();
            }
            else
            {
                if (IsMoving)
                {
                    OnStopRunning?.Invoke();
                }
                HandleIdle();
            }

            if (_desktopInput.Jump && IsGrounded) // Проверяем, на земле ли игрок
            {
                OnJumpStarted?.Invoke(); // Событие для звука прыжка
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
            IsIdle = false;
            IsMoving = true;
            OnMoving?.Invoke();
        }

        private void HandleIdle()
        {
            if (!IsIdle)
            {
                OnStartIdle?.Invoke();
            }
            IsIdle = true;
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

        public void PerformJump(bool enemyBounce = false)
        {
            if (IsGrounded || enemyBounce)
            {
                IsOnPlatform = false;
                _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
                JustJumped = false;
                IsJumping = true;
                OnJump?.Invoke();
            }
        }

        private void ApplyGravity()
        {
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

            if (IsGrounded && !JustJumped)
            {
                JustJumped = true;
            }

            if ((_characterController.collisionFlags & CollisionFlags.Above) != 0)
            {
                _velocity.y = -1f;
                _characterController.Move(new Vector3(0, -0.1f, 0));
            }
        }
    }
}
