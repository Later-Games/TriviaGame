using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame.Audio
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
    public class AudioData : ScriptableObject
    {
        public SoundType soundType;
        [Range(0f, 1f)] public float volume = 0.5f;
        [SerializeField] private List<AudioClip> audioClips;

        public AudioClip RandomClip => audioClips.RandomElement();


        public void Play(AudioSource previewer)
        {
            previewer.clip = RandomClip;
            previewer.volume = volume;
            previewer.playOnAwake = false;
            previewer.Play();
        }
    }

}