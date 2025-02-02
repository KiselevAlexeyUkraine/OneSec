using System;
using UnityEngine;

namespace Codebase.Player
{
    /// <summary>
    /// Класс для сбора ключей игроком.
    /// При попадании в триггер с объектом, принадлежащим нужному слою, ключ считается собранным.
    /// После подбора объект уничтожается.
    /// </summary>
    public class PlayerCollector : MonoBehaviour
    {
        // Слой, к которому принадлежат объекты-ключи
        [SerializeField] private LayerMask _keyMask;

        // Количество собранных ключей (закрытое поле)
        private int _keysCount = 0;

        /// <summary>
        /// Публичное свойство для получения количества ключей.
        /// </summary>
        public int KeysCount => _keysCount;

        /// <summary>
        /// Событие, которое вызывается при изменении количества ключей.
        /// Передаёт текущее количество ключей.
        /// </summary>
        public event Action<int> OnKeysChanged;

        /// <summary>
        /// Вызывается при входе в триггер.
        /// Если объект принадлежит нужному слою, увеличиваем счётчик ключей и вызываем событие.
        /// </summary>
        /// <param name="other">Другой Collider</param>
        private void OnTriggerEnter(Collider other)
        {
            // Проверяем, принадлежит ли объект заданному слою, используя LayerMask.
            if ((_keyMask.value & (1 << other.gameObject.layer)) != 0)
            {
                _keysCount++; // Увеличиваем количество собранных ключей
                OnKeysChanged?.Invoke(_keysCount); // Оповещаем подписчиков об изменении количества
                Destroy(other.gameObject); // Удаляем объект-ключ из сцены
            }
        }
    }
}
