using System.Collections.Generic;
using Codebase.Player;
using UnityEngine;

namespace Codebase.UI.Game
{
    /// <summary>
    /// ����� ��� ���������� ������������ �������� ������ � UI.
    /// ������� ���������� �������� �������� � ��������� �� ��������� � ����������� �� �������� �������� ������.
    /// </summary>
    public class UiHealth : MonoBehaviour
    {
        [SerializeField]
        private PlayerHealth _health; // ������ �� ��������� �������� ������.
        [SerializeField]
        private Transform _healthContainer; // ��������� ��� ����������� ���������� ��������� ��������.
        [SerializeField]
        private GameObject _healthPointPrefab; // ������ ��� ������������� ������ �������� ��������.

        private List<GameObject> _healthImages = new(); // ������ ��� �������� ���������� ��������� ��������.

        /// <summary>
        /// ������������� ���������� ��������� �������� � �������� �� ������� ��������� ��������.
        /// </summary>
        private void Awake()
        {
            if (_health == null || _healthContainer == null || _healthPointPrefab == null)
            {
                Debug.LogError("�� ��� ������ ������ � ����������!");
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
        /// ��������� UI �������� �� ������ �������� ���������.
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