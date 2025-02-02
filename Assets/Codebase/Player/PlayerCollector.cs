using System;
using UnityEngine;

namespace Codebase.Player
{
    /// <summary>
    /// ����� ��� ����� ������ �������.
    /// ��� ��������� � ������� � ��������, ������������� ������� ����, ���� ��������� ���������.
    /// ����� ������� ������ ������������.
    /// </summary>
    public class PlayerCollector : MonoBehaviour
    {
        // ����, � �������� ����������� �������-�����
        [SerializeField] private LayerMask _keyMask;

        // ���������� ��������� ������ (�������� ����)
        private int _keysCount = 0;

        /// <summary>
        /// ��������� �������� ��� ��������� ���������� ������.
        /// </summary>
        public int KeysCount => _keysCount;

        /// <summary>
        /// �������, ������� ���������� ��� ��������� ���������� ������.
        /// ������� ������� ���������� ������.
        /// </summary>
        public event Action<int> OnKeysChanged;

        /// <summary>
        /// ���������� ��� ����� � �������.
        /// ���� ������ ����������� ������� ����, ����������� ������� ������ � �������� �������.
        /// </summary>
        /// <param name="other">������ Collider</param>
        private void OnTriggerEnter(Collider other)
        {
            // ���������, ����������� �� ������ ��������� ����, ��������� LayerMask.
            if ((_keyMask.value & (1 << other.gameObject.layer)) != 0)
            {
                _keysCount++; // ����������� ���������� ��������� ������
                OnKeysChanged?.Invoke(_keysCount); // ��������� ����������� �� ��������� ����������
                Destroy(other.gameObject); // ������� ������-���� �� �����
            }
        }
    }
}
