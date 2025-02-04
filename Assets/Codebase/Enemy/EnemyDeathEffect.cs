using UnityEngine;
using PrimeTween;

public class EnemyDeathEffect : MonoBehaviour
{
    private BoxCollider m_BoxCollider;
    [SerializeField] private ParticleSystem m_ParticleSystem;

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_ParticleSystem.Stop();
    }

    public void Die()
    {
        GetComponent<EnemyMovement>().enabled = false; // Отключаем движение
        GetComponent<EnemyAttack>().EnemyDie();
        m_BoxCollider.enabled = false;

        Tween.LocalScale(transform, new Vector3(1f, 0.3f, 1f), 0.2f, Ease.InBounce)
            .OnComplete(() =>
                Tween.Position(transform, transform.position + Vector3.up * 0.5f, 0.2f, Ease.OutQuad)
                .OnComplete(() =>
                    Tween.Rotation(transform, Quaternion.Euler(0, 0, 180f), 0.3f, Ease.InOutQuad)
                    .OnComplete(() =>
                        Tween.LocalScale(transform, Vector3.zero, 0.3f, Ease.InBack)
                        .OnComplete(() => Destroy(gameObject))
                    )
                )
            );
    }

    public void TakeDamage()
    {
        m_ParticleSystem.Play();
    }
}
