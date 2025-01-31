using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// ����� �������� �� ���������� ��������� ���������� ������, ������������ ������ ��� �������� � ���������� ������, ����������� ��� ������ � ����.
    /// </summary>
    public class CompletePage : BasePage
    {
        [SerializeField]
        private Button _nextLevel; // ������ ��� �������� � ���������� ������.
        [SerializeField]
        private Button _restart; // ������ ��� ����������� �������� ������.
        [SerializeField]
        private Button _exit; // ������ ��� ������ � ������� ����.

        /// <summary>
        /// ������������� �� ������� ������ ��� ������������� �������.
        /// </summary>
        private void Awake()
        {
            // ������� � ��������� ����� (������).
            _nextLevel.onClick.AddListener(() => { SceneSwitcher.Instance.LoadNextScene(); });

            // ���������� ������� �����.
            _restart.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(SceneSwitcher.Instance.CurrentScene); });

            // ������� � ������� ����.
            _exit.onClick.AddListener(() => { SceneSwitcher.Instance.LoadScene(0); });

        }

        /// <summary>
        /// ������� �������� � ������� ������ ��� ����������� �������, ����� �������� ������ ������.
        /// </summary>
        private void OnDestroy()
        {
            _nextLevel.onClick.RemoveAllListeners(); // ������� ��� �������� � ������ �������� �� ��������� �������.
            _restart.onClick.RemoveAllListeners(); // ������� ��� �������� � ������ �����������.
            _exit.onClick.RemoveAllListeners(); // ������� ��� �������� � ������ ������ � ����.
        }
    }
}
