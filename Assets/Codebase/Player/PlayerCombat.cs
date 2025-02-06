using Codebase.Services.Inputs;
using Enemy;
using System;
using UnityEngine;

namespace Codebase.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public event Action OnAttack; // ��������� ������� �����

        [SerializeField] private LayerMask _enemyLayer; // ���� ������
        [SerializeField] private float _attackRange = 2f; // ��������� �����
        [SerializeField] private int _damage = 1; // ��������� ����
        [SerializeField] private Transform _attackPoint; // ����� ����� (��������, ����� ���������)
        [SerializeField] private Animator _playerAnimation;
        [SerializeField] private DesktopInput _desktopInput;

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

            _playerAnimation.SetTrigger("Shoot");

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(_damage);
                }
            }

            OnAttack?.Invoke(); // �������� ������� �����
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
