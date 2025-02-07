using UnityEngine;
using UnityEngine.Audio;

namespace Codebase.Services
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer _mixer; // Основной аудиомикшер

        private const string MasterVolumeKey = "MasterVolume";
        private const string SoundsVolumeKey = "SoundsVolume";
        private const string MusicVolumeKey = "MusicVolume";

        private const float DefaultVolume = 0.5f; // Значение по умолчанию

        public float SavedMasterVolume { get; private set; }
        public float SavedSoundsVolume { get; private set; }
        public float SavedMusicVolume { get; private set; }

        private void Awake()
        {
            LoadAudioSettings();
        }

        private void Start()
        {
            ApplyVolume();
        }

        public void SetMasterVolume(float value)
        {
            SavedMasterVolume = Mathf.Clamp(value, 0.0001f, 1f);
            SetVolumeToMixer("MasterVolume", SavedMasterVolume);
            PlayerPrefs.SetFloat(MasterVolumeKey, SavedMasterVolume);
            PlayerPrefs.Save();
        }

        public void SetSoundsVolume(float value)
        {
            SavedSoundsVolume = Mathf.Clamp(value, 0.0001f, 1f);
            SetVolumeToMixer("SoundsVolume", SavedSoundsVolume);
            PlayerPrefs.SetFloat(SoundsVolumeKey, SavedSoundsVolume);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume(float value)
        {
            SavedMusicVolume = Mathf.Clamp(value, 0.0001f, 1f);
            SetVolumeToMixer("MusicVolume", SavedMusicVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, SavedMusicVolume);
            PlayerPrefs.Save();
        }

        private void LoadAudioSettings()
        {
            SavedMasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, DefaultVolume);
            SavedSoundsVolume = PlayerPrefs.GetFloat(SoundsVolumeKey, DefaultVolume);
            SavedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultVolume);
        }

        private void ApplyVolume()
        {
            SetVolumeToMixer("MasterVolume", SavedMasterVolume);
            SetVolumeToMixer("SoundsVolume", SavedSoundsVolume);
            SetVolumeToMixer("MusicVolume", SavedMusicVolume);
        }

        private void SetVolumeToMixer(string parameter, float value)
        {
            float volume = Mathf.Log10(value) * 20f;
            _mixer.SetFloat(parameter, volume);
        }
    }
}
