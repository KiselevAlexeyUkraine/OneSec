using UnityEngine;
using System;

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
        public bool IsDie { get; private set; }

        private static readonly int DeathTrigger = Animator.StringToHash("Death");

        private void Awake()
        {
            _currentHealth = maxHealth;
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        public void TakeDamage(int amount)
        {
            if (IsDie) return; // Если враг уже мертв, урон не проходит

            _currentHealth -= amount;
            Debug.Log("Наносим урон врагу");

            OnEnemyDamaged?.Invoke();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDie) return;

            IsDie = true;
            _animator.SetTrigger(DeathTrigger); // Запуск анимации смерти

            // Отключаем атаку и движение
            if (_enemyAttack != null)
            {
                _enemyAttack.enabled = false;
            }

            // Вызываем событие смерти
            OnEnemyDied?.Invoke();

            // Очищаем события, чтобы избежать утечек памяти
            OnEnemyDied = null;
            OnEnemyDamaged = null;
        }
    }
}
