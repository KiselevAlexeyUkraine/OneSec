using UnityEngine;
using TMPro;
using Codebase.Player;

namespace Codebase.Components.Ui
{
    /// <summary>
    /// ����� ��� ���������� ������������ ���������� ��������� ������ � UI.
    /// ������������� �� ������� ��������� ���������� ������ � ������ � ��������� ��������� ������� TextMeshPro.
    /// </summary>
    public class UiKeys : MonoBehaviour
    {
        [SerializeField] private PlayerCollector _collector; // ������ �� ��������� ����� ������ ������.
        [SerializeField] private TMP_Text _keysText; // UI-����� ��� ����������� ���������� ������ (TextMeshPro).

        /// <summary>
        /// �������������: �������� ������� ������ � �������� �� ������� ��������� ���������� ������.
        /// </summary>
        private void Awake()
        {
            if (_collector == null || _keysText == null)
            {
                Debug.LogError("�� ��� ������ ������ � ���������� ��� UiKeys!");
                return;
            }

            // ������������� �� ������� ��������� ���������� ������.
            _collector.OnKeysChanged += UpdateKeysUI;
            // �������������� UI, ������ ������� ���������� ������.
            UpdateKeysUI(_collector.KeysCount);
        }

        /// <summary>
        /// ������� �� ������� ��� ����������� �������.
        /// </summary>
        private void OnDestroy()
        {
            if (_collector != null)
            {
                _collector.OnKeysChanged -= UpdateKeysUI;
            }
        }

        /// <summary>
        /// ��������� ����� UI � ����������� �� ���������� ��������� ������.
        /// </summary>
        /// <param name="keysCount">������� ���������� ������</param>
        private void UpdateKeysUI(int keysCount)
        {
            _keysText.text = "�����: " + keysCount;
        }
    }
}
