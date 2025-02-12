using UnityEngine;

namespace Codebase.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _idleClip;
        [SerializeField] private AudioClip _runClip;
        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _damageClip;
        [SerializeField] private AudioClip _deathClip;

        private PlayerMovement _movement;
        private PlayerHealth _health;

        private bool _wasJumping; // Следит за тем, когда игрок был в прыжке
        private bool _isDead; // Флаг, указывающий, умер ли игрок

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _health = GetComponent<PlayerHealth>();

            _movement.OnJumpStarted += PlayJumpSound;
            _movement.OnStartRunning += PlayRunSound;
            _movement.OnStopRunning += StopWalkingSounds;
            _movement.OnStartIdle += PlayIdleSound;
            _health.OnPlayerDied += PlayDeathSound;
            _health.OnHealthChanged += PlayDamageSound;
        }

        private void Update()
        {
            if (_isDead) return; // Если игрок мертв - не выполнять логику

            HandleGroundedSounds();
        }

        /// <summary>
        /// Проверяет, если игрок приземлился, запускает нужный звук.
        /// </summary>
        private void HandleGroundedSounds()
        {
            if (_isDead) return; // Не обрабатываем звуки после смерти
            if (_movement.IsGrounded == false)
            {
                StopWalkingSounds();
            }

            if (_movement.IsJumping)
            {
                if (!_wasJumping)
                {
                    PlayLoopingSound(_jumpClip);
                    StopWalkingSounds();
                    _wasJumping = true;
                }
            }
            else
            {
                if (_wasJumping) // Если игрок только что приземлился
                {
                    _wasJumping = false;
                    StopJumpSound();

                    if (_movement.IsMoving) // Если после приземления игрок двигается - включаем бег
                    {
                        PlayRunSound();
                    }
                    else // Если стоит - включаем Idle
                    {
                        PlayIdleSound();
                    }
                }
            }
        }

        public void PlayAttackSound()
        {
            if (_isDead) return;

            if (_attackClip != null)
            {
                _audioSource.PlayOneShot(_attackClip);
            }
        }

        private void PlayJumpSound()
        {
            if (_isDead) return;

            PlayLoopingSound(_jumpClip);
        }

        private void PlayRunSound()
        {
            if (_isDead) return;

            if (_movement.IsGrounded && !_movement.IsJumping)
            {
                PlayLoopingSound(_runClip);
            }
        }

        private void PlayIdleSound()
        {
            if (_isDead) return;

            if (_movement.IsGrounded && !_movement.IsJumping)
            {
                PlayLoopingSound(_idleClip);
            }
        }

        private void PlayDamageSound()
        {
            if (_isDead) return;

            if (_damageClip != null)
            {
                _audioSource.PlayOneShot(_damageClip);
            }
        }

        private void PlayDeathSound()
        {
            _isDead = true; // Устанавливаем флаг смерти

            StopAllSounds(); // Останавливаем все звуки перед воспроизведением звука смерти

            if (_deathClip != null)
            {
                _audioSource.PlayOneShot(_deathClip);
            }
        }

        private void PlayLoopingSound(AudioClip clip)
        {
            if (_isDead) return;

            if (_audioSource.clip == clip && _audioSource.isPlaying) return;

            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void StopWalkingSounds()
        {
            if (_audioSource.isPlaying && (_audioSource.clip == _runClip || _audioSource.clip == _idleClip))
            {
                _audioSource.Stop();
                _audioSource.clip = null;
            }
        }

        private void StopJumpSound()
        {
            if (_audioSource.isPlaying && _audioSource.clip == _jumpClip)
            {
                _audioSource.Stop();
                _audioSource.clip = null;
            }
        }

        public void StopAllSounds()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }
}
