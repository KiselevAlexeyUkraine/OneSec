using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// ����� �������� �� ���������� ��������� �������� ����, ������������ ���������� ��� ��������� ��������� � �������� � ���� �����.
    /// </summary>
    public class GameSettingsPage : BasePage
    {
        [SerializeField]
        private Button _back; // ������ ��� �������� � ���� �����.
        //[SerializeField]
        //private Slider _volume; // ������� ��� ����������� ��������� ����.

        /// <summary>
        /// ������������� �� ������� ������ � �������� ��� ������������� �������.
        /// </summary>
        private void Awake()
        {
            // ������ ��� �������� � ���� �����.
            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.Pause); });

            // ���������� ��������� �������� �������� ���������.
            //_volume.onValueChanged.AddListener(HandleVolumeChange);
        }

        /// <summary>
        /// ������� �������� � ������� ������ � �������� ��� ����������� �������, ����� �������� ������ ������.
        /// </summary>
        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners(); // ������� ��� �������� � ������ ��������.
            //_volume.onValueChanged.RemoveAllListeners(); // ������� ��� �������� � �������� ���������.
        }

    }
}
