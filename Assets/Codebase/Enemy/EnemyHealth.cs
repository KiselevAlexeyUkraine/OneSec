using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;
    private EnemyDeathEffect _deathEffect;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _deathEffect = GetComponent<EnemyDeathEffect>();
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0) _deathEffect.Die();
    }
}
