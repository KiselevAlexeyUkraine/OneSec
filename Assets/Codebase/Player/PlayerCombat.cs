using Codebase.Services.Inputs;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyLayer; // ���� ������
        [SerializeField] private float _attackRange = 2f; // ��������� �����
        [SerializeField] private int _damage = 1; // ��������� ����
        [SerializeField] private Transform _attackPoint; // ����� ����� (��������, ����� ���������)

        [SerializeField]
        private DesktopInput _desktopInput;


        private void Update()
        {
            if (_desktopInput.Fire) // ���
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
