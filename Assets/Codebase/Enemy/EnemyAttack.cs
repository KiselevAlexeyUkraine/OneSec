using UnityEngine;
using Codebase.Player;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 1; // Урон
    [SerializeField] private LayerMask _targetLayerMask; // Слой цели (например, игрок)

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!enemyHealth.IsDie)
        {
            // Проверяем, принадлежит ли объект заданному слою
            if (((1 << other.gameObject.layer) & _targetLayerMask.value) != 0)
            {
                PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {

                    // чтобы игрок подпрыгнул, даже если не считается находящимся на земле
                    playerMovement.PerformJump(true);
                }

                IDamageable target = other.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    Debug.Log("Атака врага по цели");
                }
            }
        }
    }
}
