using UnityEngine;
using System;
using Codebase.Player;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action OnEnemyAttack;

        [Header("Attack Settings")]
        [SerializeField] private int damage = 1; // Урон
        [SerializeField] private LayerMask _targetLayerMask; // Слой цели (например, игрок)
        [SerializeField] private Animator _animator; // Аниматор врага

        private EnemyHealth enemyHealth;
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
                        _animator.SetTrigger(_attackTrigger);
                        OnEnemyAttack?.Invoke();
                        target.TakeDamage(damage);
                        Debug.Log("Атака врага по цели");
                    }
                }
            }
        }
    }
}
