using UnityEngine;
using Codebase.Player;
using Codebase.Components.Ui.Pages;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PageSwitcher _pageSwitcher;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField]
    private CursorToggle cursorToggle = new();

   private void Awake()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnPlayerDied += EndLevel;
        }

    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnPlayerDied -= EndLevel;
        }
    }

    private void EndLevel()
    {
        Debug.Log("Уровень завершен. Игрок погиб.");
        Time.timeScale = 0; // Останавливаем игру
        cursorToggle.Enable();
        playerMovement.IsDie = true;
        _pageSwitcher.Open(PageName.Failed);
    }
}
