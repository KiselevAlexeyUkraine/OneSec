using UnityEngine;
using System;
using PrimeTween; // Подключаем PrimeTween

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public event Action OnEnemyDied;
        public event Action OnEnemyDamaged;

        [SerializeField] private int maxHealth = 3;
        [SerializeField] private Animator _animator;

        private int _currentHealth;
        private EnemyAttack _enemyAttack;
        private EnemyMovement _enemyMovement;
        private EnemyPatrol _enemyPatrol;
        public bool IsDead { get; private set; }

        private static readonly int DeathTrigger = Animator.StringToHash("Death");

        private void Awake()
        {
            _currentHealth = maxHealth;
            _enemyAttack = GetComponent<EnemyAttack>();
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyPatrol = GetComponent<EnemyPatrol>();

            if (_animator == null)
            {
                Debug.LogError("Animator не найден на враге!", this);
            }
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return; // Если враг уже мертв, урон не проходит

            _currentHealth -= amount;
            Debug.Log($"Нанесен урон врагу: {_currentHealth}/{maxHealth}");

            OnEnemyDamaged?.Invoke();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;

            if (_animator != null)
            {
                _animator.SetTrigger(DeathTrigger); // Запуск анимации смерти
            }

            // Отключаем атаку и движение
            if (_enemyAttack != null) _enemyAttack.enabled = false;
            if (_enemyMovement != null) _enemyMovement.enabled = false;
            if (_enemyPatrol != null) _enemyPatrol.enabled = false;

            // Вызываем событие смерти
            OnEnemyDied?.Invoke();

            // Переворачиваем врага вверх дном
            Tween.Rotation(transform, Quaternion.Euler(0, 0, 180f), 0.5f, Ease.OutBounce);

            // Очищаем события, чтобы избежать утечек памяти
            OnEnemyDied = null;
            OnEnemyDamaged = null;
        }
    }
}
