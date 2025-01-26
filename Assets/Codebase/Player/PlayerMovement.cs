using Codebase.Services.Inputs;
using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerMovementPhysics : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f; // �������� ������������
        [SerializeField] private float jumpForce = 10f; // ���� ������

        [SerializeField]
        private GroundChecker _groundChecker;

        private DesktopInput _desktopInput = new();

        private Rigidbody _rigidbody;
        private bool _isGrounded;

        private Vector3 _movementInput;
        private bool _jumpRequested;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_desktopInput == null)
            {
                Debug.LogError("DesktopInput is not assigned!");
            }

            if (_groundChecker == null)
            {
                Debug.LogError("GroundChecker is not assigned!");
            }
        }

        private void Update()
        {
            // �������� ���� ������������ ������ �� �������������� ���
            _movementInput = new Vector3(_desktopInput.Horizontal, 0f, 0f);
            _movementInput = _movementInput.normalized;

            _isGrounded = _groundChecker.IsGrounded;

            if (_desktopInput.Jump && _isGrounded)
            {
                _jumpRequested = true;
            }
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
            // ����������� �������� ������������ ������
            Vector3 moveDirection = transform.TransformDirection(_movementInput) * moveSpeed;
            Vector3 velocity = new Vector3(moveDirection.x, _rigidbody.velocity.y, _rigidbody.velocity.z);

            // ��������� �������� � Rigidbody
            _rigidbody.velocity = velocity;
        }

        private void PerformJump()
        {
            if (_isGrounded)
            {
                // ��������� ���� ������
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
