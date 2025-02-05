using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Components.Ui.Pages.Menu
{
    public class MenuPage : BasePage
    {
        [SerializeField]
        private Button _start;
        [SerializeField]
        private Button _settings;
        [SerializeField]
        private Button _authors;
        [SerializeField]
        private Button _exit;

        private void Awake()
        {
            _start.onClick.AddListener(() => { SceneSwitcher.Instance.LoadNextScene(); });
            _settings.onClick.AddListener(() => { PageSwitcher.Open(PageName.Settings); });
            _authors.onClick.AddListener(() => { PageSwitcher.Open(PageName.Authors); });
            _exit.onClick.AddListener(() => { PageSwitcher.Open(PageName.Exit); });
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _authors.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}