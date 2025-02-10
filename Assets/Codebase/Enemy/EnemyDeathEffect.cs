using UnityEngine;

namespace Enemy
{
    public class EnemyDeathEffect : MonoBehaviour
    {
        private BoxCollider _boxCollider;
        [SerializeField] private ParticleSystem _particleSystem;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _particleSystem.Stop();
        }

        public void Die()
        {
            GetComponent<EnemyMovement>().enabled = false; // Отключаем движение
            _boxCollider.enabled = false;
        }

        public void TakeDamage()
        {
            _particleSystem.Play();
        }

        // Вызывается в конце анимации смерти (Animation Event)
        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }
    }
}
