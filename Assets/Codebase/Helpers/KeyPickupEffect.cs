using UnityEngine;
using PrimeTween;

public class KeyPickupEffect : MonoBehaviour
{
    private Collider m_Collider;
    [SerializeField] private ParticleSystem pickupParticles;
    [SerializeField] private AudioSource pickupAudio;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }

    public void Pickup()
    {
        m_Collider.enabled = false; // Отключаем коллайдер

        if (pickupParticles != null)
        {
            pickupParticles.Play(); // Запускаем партиклы
        }

        if (pickupAudio != null)
        {
            pickupAudio.Play(); // Воспроизводим звук
        }

        Tween.Rotation(transform, Quaternion.Euler(0, 360f, 0), 0.5f, Ease.InOutQuad)
            .OnComplete(() =>
                Tween.Position(transform, transform.position + Vector3.up * 0.5f, 0.3f, Ease.OutQuad)
                .OnComplete(() =>
                    Tween.LocalScale(transform, Vector3.zero, 0.3f, Ease.InBack)
                    .OnComplete(() => Destroy(gameObject))
                )
            );
    }
}
