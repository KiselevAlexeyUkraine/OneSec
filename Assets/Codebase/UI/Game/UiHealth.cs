using System.Collections.Generic;
using Codebase.Player;
using UnityEngine;

namespace Codebase.UI.Game
{
    /// <summary>
    /// Класс для управления отображением здоровья игрока в UI.
    /// Создает визуальные элементы здоровья и обновляет их состояние в зависимости от текущего здоровья игрока.
    /// </summary>
    public class UiHealth : MonoBehaviour
    {
        [SerializeField]
        private PlayerHealth _health; // Ссылка на компонент здоровья игрока.
        [SerializeField]
        private Transform _healthContainer; // Контейнер для отображения визуальных элементов здоровья.
        [SerializeField]
        private GameObject _healthPointPrefab; // Префаб для представления одного элемента здоровья.

        private List<GameObject> _healthImages = new(); // Список для хранения визуальных элементов здоровья.

        /// <summary>
        /// Инициализация визуальных элементов здоровья и подписка на события изменения здоровья.
        /// </summary>
        private void Awake()
        {
            if (_health == null || _healthContainer == null || _healthPointPrefab == null)
            {
                Debug.LogError("Не все ссылки заданы в инспекторе!");
                return;
            }

            for (var i = 0; i < _health.MaxHealth; i++)
            {
                var go = Instantiate(_healthPointPrefab, _healthContainer);
                _healthImages.Add(go);
            }

            _health.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI();
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnHealthChanged -= UpdateHealthUI;
            }
        }

        /// <summary>
        /// Обновляет UI здоровья на основе текущего состояния.
        /// </summary>
        private void UpdateHealthUI()
        {
            for (int i = 0; i < _healthImages.Count; i++)
            {
                _healthImages[i].SetActive(i < _health.Health);
            }
        }
    }
}