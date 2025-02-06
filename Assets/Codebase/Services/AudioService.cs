using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Codebase.Services
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer _mixer;
        [SerializeField]
        private float _defaultVolumeScene = 0.5f;

        private const string MasterVolumePrefKey = "MasterVolume";
        private const string SoundsVolumePrefKey = "SoundsVolume";
        private const string MusicVolumePrefKey = "MusicVolume";

        public float SavedMasterVolume { get; private set; }
        public float SavedSoundsVolume { get; private set; }
        public float SavedMusicVolume { get; private set; }

        private void Awake()
        {
            SavedMasterVolume = PlayerPrefs.GetFloat(MasterVolumePrefKey, _defaultVolumeScene);
            SavedSoundsVolume = PlayerPrefs.GetFloat(SoundsVolumePrefKey, _defaultVolumeScene);
            SavedMusicVolume = PlayerPrefs.GetFloat(MusicVolumePrefKey, _defaultVolumeScene);
        }

        private void Start()
        {
            SetMasterVolume(SavedMasterVolume);
            SetSoundsVolume(SavedSoundsVolume);
            SetMusicVolume(SavedMusicVolume);
        }

        public void SetMasterVolume(float value)
        {
            SetVolume(value, MasterVolumePrefKey);
        }

        public void SetSoundsVolume(float value)
        {
            SetVolume(value, SoundsVolumePrefKey);
        }

        public void SetMusicVolume(float value)
        {
            SetVolume(value, MusicVolumePrefKey);
        }

        private void SetVolume(float value, string name)
        {
            value = Mathf.Clamp(value, 0.0001f, 1f);
            var volume = Mathf.Log10(value) * 20f;

            _mixer.SetFloat(name, volume);

            PlayerPrefs.SetFloat(name, value);
            PlayerPrefs.Save();
        }
    }
}