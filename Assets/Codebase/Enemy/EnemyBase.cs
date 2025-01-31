using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] protected int maxHealth = 3;

    protected int _currentHealth;
    protected bool _isDead;

    protected virtual void Start()
    {
        _currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
