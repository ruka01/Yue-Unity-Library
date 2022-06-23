using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yue.AudioManagement
{
    [RequireComponent(typeof(UnityEngine.Audio.AudioMixer))]
    public class AudioMixer : MonoBehaviour
    {
        public enum AudioMixMode
        {
            LinearAudioSource,
            LinearMixer,
            LogrithmicMixer
        }

        private UnityEngine.Audio.AudioMixer audioMixer;

        [SerializeField] private AudioMixMode audioMixMode;

        private void Awake()
        {
            audioMixer = GetComponent<UnityEngine.Audio.AudioMixer>();
        }

        public void LinearAudioSource(AudioSource audioSource, float volume)
        {
            audioSource.volume = volume;
        }

        public void LinearMixer(AudioSource audioSource, float volume)
        {
            audioMixer.SetFloat("Volume", (-80f + volume * 100f));
        }

        public void LogrithmicMixer(AudioSource audioSource, float volume)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20f);
        }
    }
}