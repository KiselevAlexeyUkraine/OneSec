using UnityEngine;
using Codebase.Player;
using Codebase.Components.Ui.Pages;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PageSwitcher _pageSwitcher;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private PlayerAudio _playerAudio;
    [SerializeField] private CursorToggle _cursorToggle = new();
    public bool IsLevelCompleted { get; private set; } = false; // Новый флаг завершения уровня


    private void Awake()
    {
        // Подписываемся на событие смерти игрока для завершения уровня (поражение)
        if (_playerHealth != null)
        {
            _playerHealth.OnPlayerDied += EndLevelFailure;
        }
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnPlayerDied -= EndLevelFailure;
        }
    }

    /// <summary>
    /// Завершает уровень в случае поражения (например, игрок погиб).
    /// </summary>
    private void EndLevelFailure()
    {
        Debug.Log("Уровень завершён. Игрок погиб.");
        _cursorToggle.Enable();
        _playerHealth.CompleteLevel(); // Отключаем получение урона
        _playerCombat.enabled = false;
        _playerMovement.IsDie = true;
        _pageSwitcher.Open(PageName.Failed);
    }

    /// <summary>
    /// Завершает уровень в случае победы (например, игрок подошёл к двери и имеет ключ).
    /// Этот метод можно вызвать из другого скрипта (например, скрипта двери).
    /// </summary>
    public void EndLevelVictory()
    {
        Debug.Log("Уровень завершён. Победа!");
        IsLevelCompleted = true; // Флаг завершения уровня
        _playerHealth.CompleteLevel();
        _cursorToggle.Enable();
        _playerCombat.enabled = false;
        _playerMovement.IsDie = true;
        _playerAudio.StopAllSounds();
        _pageSwitcher.Open(PageName.Complete);
    }
}
