using System.Collections.Generic;
using UnityEngine;
using Yue.DesignPatterns;

namespace Yue.AudioManagement
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<Sound> sounds = new List<Sound>();

        public new void Awake()
        {
            base.Awake();

            foreach (Sound s in sounds)
            {
                CreateSound(s.name);
            }
        }

        public void Play(string name)
        {
            Sound s = GetSound(name);

            if (s == null)
                s = CreateSound(name);

            s.source.Play();
        }

        private Sound GetSound(string name)
        {
            for (int i = 0; i < sounds.Count; ++i)
            {
                if (sounds[i].name == name)
                    return sounds[i];
            }

            Debug.LogError("Unable to find specified sound " + name);
            return null;
        }

        private Sound CreateSound(string name)
        {
            Sound s = new Sound();
            s.name = name;

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            return s;
        }

        public void StopAll()
        {
            foreach (Sound s in sounds)
            {
                if (s.source.isPlaying)
                    s.source.Stop();
            }
        }
    }
    
}