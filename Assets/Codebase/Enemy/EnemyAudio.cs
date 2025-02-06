using UnityEngine;
using System;

namespace Enemy
{
    public class EnemyAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _damageClip;
        [SerializeField] private AudioClip _deathClip;
        [SerializeField] private AudioClip _patrolClip;

        private EnemyHealth _health;
        private EnemyAttack _attack;
        private bool _isPatrolling;

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _attack = GetComponent<EnemyAttack>();

            if (_health != null)
            {
                _health.OnEnemyDied += PlayDeathSound;
                _health.OnEnemyDamaged += PlayDamageSound;
            }

            if (_attack != null)
            {
                _attack.OnEnemyAttack += PlayAttackSound;
            }

            StartPatrolSound();
        }

        private void StartPatrolSound()
        {
            if (_audioSource != null && _patrolClip != null)
            {
                _audioSource.clip = _patrolClip;
                _audioSource.loop = true;
                _audioSource.Play();
                _isPatrolling = true;
            }
        }

        public void PlayAttackSound()
        {
            if (_attackClip != null)
            {
                _audioSource.clip = _attackClip;
                _audioSource.loop = false;
                _audioSource.Play();
                Invoke(nameof(StartPatrolSound), _attackClip.length);
            }
        }

        private void PlayDamageSound()
        {
            if (_damageClip != null)
            {
                _audioSource.clip = _damageClip;
                _audioSource.loop = false;
                _audioSource.Play();
            }
        }

        private void PlayDeathSound()
        {
            if (_deathClip != null)
            {
                _audioSource.clip = _deathClip;
                _audioSource.loop = false;
                _audioSource.Play();
            }
        }
    }
}
