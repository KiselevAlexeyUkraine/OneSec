using Codebase.Player;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 1; // Урон
    [SerializeField] private float attackCooldown = 1f; // Время между атаками
    [SerializeField] private LayerMask _targetLayerMask; // Слой цели (например, игрок)

    private EnemyHealth enemyHealth;
    private float _lastAttackTime;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enemyHealth.IsDie && Time.time > _lastAttackTime + attackCooldown)
        {
            // Проверяем, принадлежит ли объект заданному слою
            if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
            {
                other.GetComponent<PlayerMovement>().PerformJump();
                IDamageable target = other.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    _lastAttackTime = Time.time; // Обновляем время атаки
                    Debug.Log("Атака врага по цели");
                }
            }
        }
    }
}
