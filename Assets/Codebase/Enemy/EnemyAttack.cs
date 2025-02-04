using UnityEngine;
using Codebase.Player;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 1; // Урон
    [SerializeField] private LayerMask _targetLayerMask; // Слой цели (например, игрок)
    [SerializeField] private Animator _animator; // Аниматор врага
    [SerializeField] private AudioSource _audioSource; // Универсальный аудиофайл
    [SerializeField] private AudioClip _attackClip; // Звук атаки
    [SerializeField] private AudioClip _deathClip; // Звук атаки
    [SerializeField] private AudioClip _patrolClip; // Звук патрулирования

    private EnemyHealth enemyHealth;
    private bool isPatrolling = true;

    private readonly int _attackTrigger = Animator.StringToHash("Attack");

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator не найден на враге!");
            }
        }

        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("AudioSource не найден на враге!");
            }
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
            isPatrolling = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemyHealth.IsDie)
        {
            if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
            {
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.PerformJump(true);
                }

                IDamageable target = other.GetComponent<IDamageable>();
                if (target != null)
                {
                    if (_audioSource != null && _audioSource.isPlaying)
                    {
                        _audioSource.Stop();
                    }

                    _animator.SetTrigger(_attackTrigger);

                    if (_audioSource != null && _attackClip != null)
                    {
                        _audioSource.clip = _attackClip;
                        _audioSource.loop = false;
                        _audioSource.Play();
                        Invoke(nameof(StartPatrolSound), _attackClip.length); // Возвращаем патрульный звук после атаки
                    }

                    target.TakeDamage(damage);
                    Debug.Log("Атака врага по цели");
                }
            }
        }
    }

    public void EnemyDie()
    {
        if (_audioSource != null && _deathClip != null)
        {
            _audioSource.clip = _deathClip;
            _audioSource.loop = false;
            _audioSource.Play();
        }
    }

    public void EnemyTakeDamageSoud()
    {
        if (_audioSource != null && _attackClip != null)
        {
            _audioSource.clip = _attackClip;
            _audioSource.loop = false;
            _audioSource.Play();
            Invoke(nameof(StartPatrolSound), _attackClip.length); // Возвращаем патрульный звук после атаки
        }
    }
}
