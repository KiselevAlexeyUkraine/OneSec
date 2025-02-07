using Codebase.Services.Inputs;
using Enemy;
using System;
using UnityEngine;

namespace Codebase.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public event Action OnAttack;

        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _attackRange = 2f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Animator _playerAnimation;
        [SerializeField] private DesktopInput _desktopInput;
        [SerializeField] private float _attackCooldownTime = 1f; // Время задержки между атаками

        private static readonly int AttackTrigger = Animator.StringToHash("Shoot");
        private bool _isAttacking;
        private float _attackCooldown;

        private void Update()
        {
            _attackCooldown -= Time.deltaTime;

            if (_desktopInput.Fire && !_isAttacking && _attackCooldown <= 0f)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _isAttacking = true;
            _attackCooldown = _attackCooldownTime; // Устанавливаем кулдаун атаки
            PerformAttack();
        }

        private void PerformAttack()
        {
            _playerAnimation.SetTrigger(AttackTrigger);

            Collider[] hitEnemies = Physics.OverlapSphere(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(_damage);
                }
            }

            OnAttack?.Invoke();
            _isAttacking = false;
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
