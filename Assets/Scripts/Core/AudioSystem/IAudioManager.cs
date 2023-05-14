using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace ZadanieRekrutacyjne.Core.AudioSystem
{
    public interface IAudioManager
    {
        void Play(string request, float? volume = 1f, float? pitch = 1f, int? priority = 128, bool? loop = false);
        void Play(string request, AudioMixerGroup mixer, float volume = 1f, float pitch = 1f);
        void Play(string request, int mixerID, float volume = 1f, float pitch = 1f);
        void Play(string request, Hashtable args);
        AudioSource PlayAndGetSource(string request, float volume = 1f, float pitch = 1f, int? priority = 128, bool? loop = false);
        AudioSource PlayAndGetSource(string request, AudioMixerGroup mixer, float volume = 1f, float pitch = 1f);
        AudioSource PlayAndGetSource(string request, int mixerID, float volume = 1f, float pitch = 1f);
        AudioSource PlayAndGetSource(string request, Hashtable args);
    }
}
