using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;
    private EnemyDeathEffect _deathEffect;
    private EnemyAttack enemyAttack;
    public bool IsDie { get; private set; }

    private void Awake()
    {
        _currentHealth = maxHealth;
        _deathEffect = GetComponent<EnemyDeathEffect>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    public void TakeDamage(int amount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= amount;
            Debug.Log("Наносим урон врагу");
            _deathEffect.TakeDamage();
            enemyAttack.EnemyTakeDamageSoud();
            if (_currentHealth <= 0)
            {
                _deathEffect.Die();
                IsDie = true;
            }

        }

    }
}
