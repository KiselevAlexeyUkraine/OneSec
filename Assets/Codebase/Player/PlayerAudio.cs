using UnityEngine;

namespace Codebase.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _landClip;
        [SerializeField] private AudioClip _damageClip;
        [SerializeField] private AudioClip _deathClip;

        private PlayerMovement _movement;
        private PlayerCombat _combat;
        private PlayerHealth _health;
        private bool _wasGrounded;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _combat = GetComponent<PlayerCombat>();
            _health = GetComponent<PlayerHealth>();

            _movement.OnJump += PlayJumpSound;
            _combat.OnAttack += PlayAttackSound;
            _health.OnPlayerDied += PlayDeathSound;
            _health.OnHealthChanged += PlayDamageSound;
        }

        private void Update()
        {
            HandleLandingSound();
        }

        private void PlayAttackSound()
        {
            if (_attackClip != null)
            {
                _audioSource.clip = _attackClip;
                _audioSource.Play();
            }
        }

        private void PlayJumpSound()
        {
            if (_movement.IsGrounded && _jumpClip != null)
            {
                _audioSource.clip = _jumpClip;
                _audioSource.Play();
            }
        }

        private void HandleLandingSound()
        {
            if (!_wasGrounded && _movement.IsGrounded && _landClip != null)
            {
                _audioSource.clip = _landClip;
                _audioSource.Play();
            }
            _wasGrounded = _movement.IsGrounded;
        }

        private void PlayDamageSound()
        {
            if (_damageClip != null)
            {
                _audioSource.clip = _damageClip;
                _audioSource.Play();
            }
        }

        private void PlayDeathSound()
        {
            if (_deathClip != null)
            {
                _audioSource.clip = _deathClip;
                _audioSource.Play();
            }
        }
    }
}
