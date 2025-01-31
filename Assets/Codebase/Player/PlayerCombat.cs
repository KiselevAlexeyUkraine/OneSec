using Codebase.Services.Inputs;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyLayer; // Слой врагов
        [SerializeField] private float _attackRange = 2f; // Дистанция удара
        [SerializeField] private int _damage = 1; // Наносимый урон
        [SerializeField] private Transform _attackPoint; // Точка атаки (например, центр персонажа)

        [SerializeField]
        private DesktopInput _desktopInput;


        private void Update()
        {
            if (_desktopInput.Fire) // ЛКМ
            {
                Attack();
                
            }
        }

        private void Attack()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(_damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_attackPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
            }
        }
    }
}
