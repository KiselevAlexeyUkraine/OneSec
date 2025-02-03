using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Components.Ui.Pages.Game

{
    /// <summary>
    /// Класс отвечает за управление страницей настроек игры, предоставляя функционал для изменения громкости и возврата в меню паузы.
    /// </summary>
    public class GameSettingsPage : BasePage
    {
        [SerializeField]
        private Button _back; // Кнопка для возврата в меню паузы.
        //[SerializeField]
        //private Slider _volume; // Слайдер для регулировки громкости игры.

        /// <summary>
        /// Подписываемся на события кнопок и слайдера при инициализации объекта.
        /// </summary>
        private void Awake()
        {
            // Кнопка для возврата в меню паузы.
            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.Pause); });

            // Обработчик изменения значения слайдера громкости.
            //_volume.onValueChanged.AddListener(HandleVolumeChange);
        }

        /// <summary>
        /// Убираем подписки с событий кнопок и слайдера при уничтожении объекта, чтобы избежать утечек памяти.
        /// </summary>
        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners(); // Убираем все подписки с кнопки возврата.
            //_volume.onValueChanged.RemoveAllListeners(); // Убираем все подписки с слайдера громкости.
        }

    }
}
