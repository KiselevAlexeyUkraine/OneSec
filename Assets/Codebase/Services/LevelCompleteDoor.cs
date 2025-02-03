using UnityEngine;
using Codebase.Player; // Предполагается, что PlayerCollector находится в этом пространстве имён

public class LevelCompleteDoor : MonoBehaviour
{
    [Header("Настройки завершения уровня")]
    [SerializeField] private LayerMask _playerLayerMask; // Слой, определяющий игрока
    [SerializeField] private Animator _doorAnimator;       // Аниматор двери (если используется анимация)
    [SerializeField] private string _openTrigger = "Open";   // Имя триггера для анимации открытия двери
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private AudioSource _doorAudio; // Аудио для звука открытия двери
    [SerializeField] private AudioClip _doorOpenClip; // Звук открытия двери

    private bool _isDoorOpened = false;

    /// <summary>
    /// При входе объекта в область завершения уровня проверяем, является ли он игроком.
    /// Если у игрока есть хотя бы один ключ, дверь открывается.
    /// </summary>
    /// <param name="other">Другой Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, принадлежит ли объект заданному слою
        if (((1 << other.gameObject.layer) & _playerLayerMask.value) != 0 && !_isDoorOpened)
        {
            // Пытаемся получить компонент PlayerCollector у игрока
            PlayerCollector collector = other.GetComponent<PlayerCollector>();
            if (collector != null)
            {
                if (collector.KeysCount > 0)
                {
                    // Если у игрока есть ключ(и) - открываем дверь
                    OpenDoor();
                    _levelManager.EndLevelVictory();
                }
                else
                {
                    Debug.Log("У игрока нет ключей для открытия двери!");
                }
            }
            else
            {
                Debug.LogWarning("На объекте игрока не найден компонент PlayerCollector!");
            }
        }
    }

    /// <summary>
    /// Открывает дверь (запускает анимацию или выполняет другую логику завершения уровня).
    /// </summary>
    private void OpenDoor()
    {
        _isDoorOpened = true;
        if (_doorAnimator != null)
        {
            _doorAnimator.SetTrigger(_openTrigger);
        }
        else
        {
            Debug.Log("Дверь открыта (аниматор не назначен)!");
        }

        // Воспроизведение звука открытия двери
        if (_doorAudio != null && _doorOpenClip != null)
        {
            _doorAudio.PlayOneShot(_doorOpenClip);
        }

        // Дополнительная логика завершения уровня (например, загрузка следующей сцены)
        Debug.Log("Уровень завершён!");

    }
}
