using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Components.Ui.Pages.Game

{
    /// <summary>
    /// Класс отвечает за управление страницей "Неудачи", предоставляя кнопки для перезапуска уровня или выхода в меню.
    /// </summary>
    public class FailedPage : BasePage
    {
        [SerializeField]
        private Button _restart; // Кнопка для перезапуска текущего уровня.
        [SerializeField]
        private Button _exit; // Кнопка для перехода в главное меню.

        /// <summary>
        /// Подписываемся на события кнопок при инициализации объекта.
        /// </summary>
        private void Awake()
        {
            // Перезапуск текущей сцены (уровня).
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(SceneSwitcher.Instance.CurrentScene); });

            // Переход в главное меню.
            _exit.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(0); });

        }

        /// <summary>
        /// Убираем подписки с событий кнопок при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            _restart.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки перезапуска.
            _exit.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки выхода в меню.
        }
    }
}
