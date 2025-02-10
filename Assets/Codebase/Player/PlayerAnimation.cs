using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerMovement _movement;
        private Animator _animator;
        private PlayerHealth _health;

        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<PlayerMovement>();
            _health = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            _health.OnPlayerDied += PlayDeathAnimation;
        }

        private void OnDisable()
        {
            _health.OnPlayerDied -= PlayDeathAnimation;
        }

        private void Update()
        {
            // Устанавливаем состояние движения
            _animator.SetBool(IsMoving, _movement.IsMoving);

            // Устанавливаем состояние прыжка и приземления
            _animator.SetBool(IsJumping, _movement.IsJumping);
            _animator.SetBool(IsGrounded, _movement.IsGrounded);
        }

        private void PlayDeathAnimation()
        {
            _animator.SetTrigger(IsDead);
        }
    }
}
