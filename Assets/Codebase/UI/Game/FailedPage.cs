using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// ����� �������� �� ���������� ��������� "�������", ������������ ������ ��� ����������� ������ ��� ������ � ����.
    /// </summary>
    public class FailedPage : BasePage
    {
        [SerializeField]
        private Button _restart; // ������ ��� ����������� �������� ������.
        [SerializeField]
        private Button _exit; // ������ ��� �������� � ������� ����.

        /// <summary>
        /// ������������� �� ������� ������ ��� ������������� �������.
        /// </summary>
        private void Awake()
        {
            // ���������� ������� ����� (������).
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(SceneSwitcher.Instance.CurrentScene); });

            // ������� � ������� ����.
            _exit.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(0); });

        }

        /// <summary>
        /// ������� �������� � ������� ������ ��� ����������� �������, ����� �������� ������ ������.
        /// </summary>
        private void OnDestroy()
        {
            _restart.onClick.RemoveAllListeners(); // ������� ��� �������� � ������ �����������.
            _exit.onClick.RemoveAllListeners(); // ������� ��� �������� � ������ ������ � ����.
        }
    }
}
