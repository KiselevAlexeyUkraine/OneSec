using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 1; // Урон
    [SerializeField] private float attackCooldown = 1f; // Время между атаками
    [SerializeField] private string targetTag = "Player"; // Тэг цели (по умолчанию - игрок)

    private EnemyHealth enemyHealth;

    private float _lastAttackTime;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyHealth.IsDie == false)
        {
            if (Time.time > _lastAttackTime + attackCooldown && other.CompareTag(targetTag))
            {
                IDamageable target = other.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    _lastAttackTime = Time.time; // Обновляем время атаки
                    Debug.Log("Мы прикоснулись к пауку");
                }
            }
        }
    }
}
