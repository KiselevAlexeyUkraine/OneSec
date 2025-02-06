using UnityEngine;
using System;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public event Action OnEnemyDied;
        public event Action OnEnemyDamaged;

        [SerializeField] private int maxHealth = 3;
        private int _currentHealth;
        private EnemyDeathEffect _deathEffect;
        public bool IsDie { get; private set; }

        private void Awake()
        {
            _currentHealth = maxHealth;
            _deathEffect = GetComponent<EnemyDeathEffect>();

            // Подписываем метод Die() на событие OnEnemyDied (без вызова!)
            OnEnemyDied += _deathEffect.Die;
        }

        public void TakeDamage(int amount)
        {
            if (_currentHealth > 0)
            {
                _currentHealth -= amount;
                Debug.Log("Наносим урон врагу");
                _deathEffect.TakeDamage();
                OnEnemyDamaged?.Invoke();

                if (_currentHealth <= 0)
                {
                    IsDie = true;

                    // Вызываем событие, которое запустит подписанные методы (например, _deathEffect.Die)
                    OnEnemyDied?.Invoke();
                }
            }
        }
    }
}
