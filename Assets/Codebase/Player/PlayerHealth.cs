using System;
using UnityEngine;

namespace Codebase.Player
{
    /// <summary>
    /// Класс отвечает за управление здоровьем игрока, его увеличением, уменьшением и связными событиями.
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public event Action OnHealthChanged;
        public event Action OnPlayerDied; // Событие смерти игрока

        [SerializeField]
        private int _maxHealth = 5;
        [SerializeField]
        private int _health = 5;

        public int MaxHealth => _maxHealth;
        public int Health => _health;

        private void Awake()
        {
            _health = _maxHealth;
        }

        public void IncreaseHealth(int amount = 1)
        {
            if (amount > 0)
            {
                _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
                OnHealthChanged?.Invoke();
            }
        }

        public void DecreaseHealth(int amount = 1)
        {
            if (amount > 0)
            {
                _health = Mathf.Clamp(_health - amount, 0, _maxHealth);
                OnHealthChanged?.Invoke();

                if (_health <= 0)
                {
                    Die();
                }
            }
        }

        public void TakeDamage(int amount)
        {
            DecreaseHealth(amount);
        }

        private void Die()
        {
            Debug.Log("Игрок умер!");
            OnPlayerDied?.Invoke();
            // Здесь можно добавить респаун или завершение игры
        }
    }
}
