using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _hoverSound;

        private void Awake()
        {
            _start.onClick.AddListener(() => { SceneSwitcher.Instance.LoadNextScene(); });
            _settings.onClick.AddListener(() => { PageSwitcher.Open(PageName.Settings); });
            _authors.onClick.AddListener(() => { PageSwitcher.Open(PageName.Authors); });
            _exit.onClick.AddListener(() => { PageSwitcher.Open(PageName.Exit); });

            AddHoverSound(_start);
            AddHoverSound(_settings);
            AddHoverSound(_authors);
            AddHoverSound(_exit);
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners();
            _authors.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }

        private void AddHoverSound(Button button)
        {
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entry.callback.AddListener((_) => PlayHoverSound());
            trigger.triggers.Add(entry);
        }

        private void PlayHoverSound()
        {
            if (_audioSource != null && _hoverSound != null)
            {
                _audioSource.PlayOneShot(_hoverSound);
            }
        }
    }
}
