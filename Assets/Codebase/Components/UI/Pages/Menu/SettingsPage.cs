using Codebase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Codebase.Components.Ui.Pages.Menu
{
    public class SettingsPage : BasePage
    {
        [SerializeField]
        private Button _back;
        [SerializeField]
        private Slider _masterVolume;
        [SerializeField]
        private Slider _soundsVolume;
        [SerializeField]
        private Slider _musicVolume;

        private AudioService _audioService;

        [Inject]
        private void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }

        private void Awake()
        {
            
            if (_audioService == null)
            {
                Debug.Log("Аудио сорсе равен нулю");
            }


            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.Menu); });
            _masterVolume.onValueChanged.AddListener(value => _audioService.SetMasterVolume(value));
            _soundsVolume.onValueChanged.AddListener(value => _audioService.SetSoundsVolume(value));
            _musicVolume.onValueChanged.AddListener(value => _audioService.SetMusicVolume(value));

            _masterVolume.value = _audioService.SavedMasterVolume;
            _soundsVolume.value = _audioService.SavedSoundsVolume;
            _musicVolume.value = _audioService.SavedMusicVolume;
        }

        private void OnDestroy()
        {
            _back.onClick.RemoveAllListeners();
            _masterVolume.onValueChanged.RemoveAllListeners();
            _soundsVolume.onValueChanged.RemoveAllListeners();
            _musicVolume.onValueChanged.RemoveAllListeners();
        }
    }
}