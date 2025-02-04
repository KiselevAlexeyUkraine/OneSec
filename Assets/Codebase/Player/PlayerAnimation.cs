using UnityEngine;

namespace Codebase.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerMovement _movement;
        private Animator _animator;

        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            // Устанавливаем состояние движения
            _animator.SetBool(IsMoving, _movement.IsMoving);

            // Устанавливаем состояние прыжка и приземления
            _animator.SetBool(IsJumping, _movement.IsJumping);
            _animator.SetBool(IsGrounded, _movement.IsGrounded);
        }
    }
}
