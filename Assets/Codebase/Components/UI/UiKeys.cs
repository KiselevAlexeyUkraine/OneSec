using UnityEngine;
using TMPro;
using Codebase.Player;

namespace Codebase.Components.Ui
{
    /// <summary>
    /// Класс для управления отображением количества собранных ключей в UI.
    /// Подписывается на событие изменения количества ключей у игрока и обновляет текстовый элемент TextMeshPro.
    /// </summary>
    public class UiKeys : MonoBehaviour
    {
        [SerializeField] private PlayerCollector _collector; // Ссылка на компонент сбора ключей игрока.
        [SerializeField] private TMP_Text _keysText; // UI-текст для отображения количества ключей (TextMeshPro).

        /// <summary>
        /// Инициализация: проверка наличия ссылок и подписка на событие изменения количества ключей.
        /// </summary>
        private void Awake()
        {
            if (_collector == null || _keysText == null)
            {
                Debug.LogError("Не все ссылки заданы в инспекторе для UiKeys!");
                return;
            }

            // Подписываемся на событие изменения количества ключей.
            _collector.OnKeysChanged += UpdateKeysUI;
            // Инициализируем UI, выводя текущее количество ключей.
            UpdateKeysUI(_collector.KeysCount);
        }

        /// <summary>
        /// Отписка от событий при уничтожении объекта.
        /// </summary>
        private void OnDestroy()
        {
            if (_collector != null)
            {
                _collector.OnKeysChanged -= UpdateKeysUI;
            }
        }

        /// <summary>
        /// Обновляет текст UI в зависимости от количества собранных ключей.
        /// </summary>
        /// <param name="keysCount">Текущее количество ключей</param>
        private void UpdateKeysUI(int keysCount)
        {
            _keysText.text = "Ключи: " + keysCount;
        }
    }
}
