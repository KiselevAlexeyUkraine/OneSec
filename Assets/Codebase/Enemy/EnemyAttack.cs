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
        [SerializeField] private Transform attackPoint; // Точка атаки
        [SerializeField] private float attackRadius = 1.5f; // Радиус атаки

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
            if (!enemyHealth.IsDie && ((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
            {
                _animator.SetTrigger(_attackTrigger);
            }
        }

        // Метод, вызываемый из анимации удара паука
        public void PerformSpiderAttack()
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius, _targetLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                IDamageable target = hitCollider.GetComponent<IDamageable>();
                if (target != null)
                {
                    OnEnemyAttack?.Invoke();
                    target.TakeDamage(damage);
                    Debug.Log("Атака врага по цели");
                }
                PlayerMovement playerMovement = hitCollider.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.PerformJump(true);
                }
            }
        }

        // Отображение радиуса атаки в редакторе сцены
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
