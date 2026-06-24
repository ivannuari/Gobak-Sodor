using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string key;
        public AudioClip clip;
        public bool isLoop;
        public AudioSource source;
    }

    public Sound[] allSounds;

    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        foreach (var item in allSounds)
        {
            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.clip;
            item.source.loop = item.isLoop;

            if(item.isLoop)
            {
                item.source.volume = bgmVolume;
            }
            else
            {
                item.source.volume = sfxVolume;
            }
        }
    }

    public void PlaySound(string key)
    {
        Sound findSound = Array.Find(allSounds, item => item.key == key);
        if (findSound != null)
        {
            findSound.source.Play();
        }
    }

    public void ChangeBgmVolume(float val)
    {
        bgmVolume = val;

        Sound[] allBgm = Array.FindAll(allSounds, s => s.isLoop);
        foreach (var item in allBgm)
        {
            item.source.volume = bgmVolume;
        }
    }

    public void ChangeSfxVolume(float val)
    {
        sfxVolume = val;

        Sound[] allSfx = Array.FindAll(allSounds, s => !s.isLoop);
        foreach (var item in allSfx)
        {
            item.source.volume = sfxVolume;
        }
    }
}