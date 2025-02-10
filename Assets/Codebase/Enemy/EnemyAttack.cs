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
        [SerializeField] private EnemyMovement _movement; // Компонент движения

        private EnemyHealth _enemyHealth;
        private PlayerHealth _playerHealth; // Ссылка на здоровье игрока
        private readonly int _attackTrigger = Animator.StringToHash("Attack");
        private bool _isAttacking = false; // Флаг атаки

        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _movement = GetComponent<EnemyMovement>();

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
            if (_playerHealth == null || _playerHealth.Health <= 0) return; // Не атаковать, если игрок мертв

            if (!_enemyHealth.IsDie && !_isAttacking && ((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
            {
                StartAttack();
            }
        }

        private void StartAttack()
        {
            _isAttacking = true;
            _movement.SetMovementEnabled(false); // Останавливаем движение
            _animator.SetTrigger(_attackTrigger);
        }

        // Метод, вызываемый из анимации удара (Animation Event)
        public void PerformSpiderAttack()
        {
            if (_playerHealth == null || _playerHealth.Health <= 0) return; // Проверка на смерть игрока

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

        // Метод вызывается в конце анимации атаки (Animation Event)
        public void EndAttack()
        {
            _isAttacking = false;
            _movement.SetMovementEnabled(true); // Разрешаем движение врага
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
