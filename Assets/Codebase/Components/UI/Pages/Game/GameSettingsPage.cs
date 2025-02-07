using Codebase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Codebase.Components.Ui.Pages.Game
{
    public class GameSettingsPage : BasePage
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
            _back.onClick.AddListener(() => { PageSwitcher.Open(PageName.Pause).Forget(); });

            _masterVolume.onValueChanged.AddListener(_audioService.SetMasterVolume);
            _soundsVolume.onValueChanged.AddListener(_audioService.SetSoundsVolume);
            _musicVolume.onValueChanged.AddListener(_audioService.SetMusicVolume);

            LoadSettings();
        }

        private void LoadSettings()
        {
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
