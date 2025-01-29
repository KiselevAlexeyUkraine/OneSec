using System;
using UnityEngine;

namespace Codebase.Player
{
    /// <summary>
    /// Класс отвечает за управление здоровьем игрока, его увеличением, уменьшением и связными событиями.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        public event Action OnHealthChanged;

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
            }
        }
    }
}
