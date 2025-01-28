using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerMovement _movement;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<PlayerMovement>();
        }

        // Кэшированные хэши параметров анимации для повышения производительности.
        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _canJump = Animator.StringToHash("CanJump");
        private readonly int _isGrounded = Animator.StringToHash("IsGrounded");

        // Обновляет состояния анимации каждый кадр.
        private void Update()
        {
            _animator.SetBool(_isMoving, _movement.IsMoving);
            _animator.SetBool(_idle, _movement.Idle);
            _animator.SetBool(_canJump, _movement.CanJump);
            _animator.SetBool(_isGrounded, _movement.IsGrounded);
        }
    }
}
