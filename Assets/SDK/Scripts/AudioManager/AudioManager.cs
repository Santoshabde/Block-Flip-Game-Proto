using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class AudioManager : SerializeSingleton<AudioManager>
    {
        [SerializeField] private AudioSource gameBG;
        [SerializeField] private List<AudioData> audioData;

        private Dictionary<string, AudioClip> AudioData;

        public bool shouldPlaySound;
        public bool shouldVibrate;

        private void Awake()
        {
            AudioData = new Dictionary<string, AudioClip>();

            foreach (var item in audioData)
            {
                AudioData.Add(item.audioID, item.audioClip);
            }

            shouldPlaySound = true;
            shouldVibrate = true;
        }

        private void Update()
        {
            gameBG.enabled = shouldPlaySound;
        }

        public void PlayAudioClipWithAutoDestroy(string id)
        {
            if (!shouldPlaySound)
                return;

            if (AudioData.ContainsKey(id))
            {
                GameObject audio = new GameObject();
                audio.AddComponent<AudioSource>();
                audio.GetComponent<AudioSource>().PlayOneShot(AudioData[id]);

                float length = AudioData[id].length + 1;
                Destroy(audio, length);
            }
            else
            {
                Debug.LogError("[AudioManager] Mentioned soundid is not present");
            }
        }

        public void PlayAudioClip(string id, float destoryTime)
        {
            if (!shouldPlaySound)
                return;

            if (AudioData.ContainsKey(id))
            {
                GameObject audio = new GameObject();
                audio.AddComponent<AudioSource>();
                audio.GetComponent<AudioSource>().PlayOneShot(AudioData[id]);

                Destroy(audio, destoryTime);
            }
            else
            {
                Debug.LogError("[AudioManager] Mentioned soundid is not present");
            }
        }

        public void PlayVibration()
        {
            if (!shouldVibrate)
                return;

#if !UNITY_WEBGL
            Handheld.Vibrate();
#endif
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public string audioID;
        public AudioClip audioClip;
    }
}
