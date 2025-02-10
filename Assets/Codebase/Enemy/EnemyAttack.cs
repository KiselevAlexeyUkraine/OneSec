using UnityEngine;
using System;
using Codebase.Player;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action OnEnemyAttack;

        [Header("Attack Settings")]
        [SerializeField] private int damage = 1;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRadius = 1.5f;
        [SerializeField] private EnemyMovement _movement;

        private EnemyHealth _enemyHealth;
        private PlayerHealth _playerHealth;
        private LevelManager _levelManager;
        private bool _isAttacking = false;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _movement = GetComponent<EnemyMovement>();
            _levelManager = FindObjectOfType<LevelManager>();

            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                {
                    Debug.LogError("Animator не найден на враге!");
                }
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerHealth = player.GetComponent<PlayerHealth>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_playerHealth == null || _playerHealth.Health <= 0 || _levelManager == null || _levelManager.IsLevelCompleted) return;
            if (_enemyHealth.IsDie || _isAttacking) return;

            if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
            {
                StartAttack();
            }
        }

        private void StartAttack()
        {
            _isAttacking = true;
            //_movement.SetMovementEnabled(false);
            _animator.SetBool(AttackTrigger, true);
        }

        // Вызывается из анимации атаки
        public void PerformSpiderAttack()
        {
            if (_playerHealth == null || _playerHealth.Health <= 0 || _levelManager == null || _levelManager.IsLevelCompleted) return;

            Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius, _targetLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageable target))
                {
                    OnEnemyAttack?.Invoke();
                    target.TakeDamage(damage);
                    Debug.Log("Атака врага по цели");
                }

                if (hitCollider.TryGetComponent(out PlayerMovement playerMovement))
                {
                    playerMovement.PerformJump(true);
                }
            }
        }

        // Вызывается в конце анимации атаки
        public void EndAttack()
        {
            _isAttacking = false;
            //_movement.SetMovementEnabled(true);
            _animator.SetBool(AttackTrigger, false);
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
            }
        }
    }
}
