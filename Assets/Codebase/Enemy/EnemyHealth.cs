using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;
    private EnemyDeathEffect _deathEffect;
    public bool IsDie { get; private set; }

    private void Awake()
    {
        _currentHealth = maxHealth;
        _deathEffect = GetComponent<EnemyDeathEffect>();
    }

    public void TakeDamage(int amount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= amount;
            Debug.Log("Наносим урон врагу");
            _deathEffect.TakeDamage();
            if (_currentHealth <= 0)
            {
                _deathEffect.Die();
                IsDie = true;
            }

        }

    }
}
