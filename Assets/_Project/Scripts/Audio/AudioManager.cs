using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TriviaGame.Audio
{
   public class AudioManager : MonoBehaviour
   {
      [SerializeField] private Settings settings;
      [SerializeField] private AudioMixer audioMixer;

      [Space(10)] 
      [Header("Fx")] 
      [SerializeField] private List<AudioSource> fxSourceList;

      [Space(10)] 
      [Header("Music")] 
      [SerializeField] private AudioSource musicSource;

      public static AudioManager instance;
      public event Action<bool> OnSoundStateChanged;
      public event Action<bool> OnMusicStateChanged;

      private Dictionary<SoundType, AudioData> _typeToData = new Dictionary<SoundType, AudioData>();
      private Dictionary<SoundType, AudioSource> _typeToSource = new Dictionary<SoundType, AudioSource>();
      private Dictionary<AudioSource, SoundType> _sourceToType = new Dictionary<AudioSource, SoundType>();

      private int _fxSourceIndex = 0;

      private void Awake()
      {
         // Singleton
         if (instance == null)
         {
            instance = this;
         }
         else if (instance != this)
         {
            Destroy(gameObject);
         }
         
         // Setup data dictionary
         foreach (var data in settings.audioLibrary.audioDataList)
         {
            _typeToData[data.soundType] = data;
         }
      }

      private void Start()
      {
         // Set initial sound states

         bool soundState = SoundOn;
         SoundOn = soundState;

         bool musicState = MusicOn;
         MusicOn = musicState;

         // Start background music
         PlayRandomMusic();
      }

      public void PlaySoundFx(SoundType soundType)
      {
         // Get next audio source in list and use it with target data
         fxSourceList[_fxSourceIndex].clip = _typeToData[soundType].RandomClip;
         fxSourceList[_fxSourceIndex].volume = _typeToData[soundType].volume;
         fxSourceList[_fxSourceIndex].Play();
         
         // Add new data and source entries to dictionaries to early stop sounds in need
         _typeToSource[soundType] = fxSourceList[_fxSourceIndex];
         _sourceToType[fxSourceList[_fxSourceIndex]] = soundType;

         // Increase current index
         _fxSourceIndex = (_fxSourceIndex + 1) % fxSourceList.Count;
      }

      public void StopSoundFx(SoundType soundType)
      {
         // Check if target sound played before with a source and source is still playing with the same sound
         // Then stop audio source
         if (_typeToSource.ContainsKey(soundType) && _sourceToType[_typeToSource[soundType]] == soundType)
         {
            _typeToSource[soundType].Stop();
         }
      }

      void PlayRandomMusic()
      {
         // Pick a random music data
         AudioData randomData = settings.audioLibrary.musicDataList.RandomElement();
         
         // Setup music audio source with data
         musicSource.clip = randomData.RandomClip;
         musicSource.volume = randomData.volume;
         musicSource.Play();

         // Setup another random music when current music ends
         Invoke(nameof(PlayRandomMusic), musicSource.clip.length);
      }

      public bool SoundOn
      {
         get => PlayerPrefs.GetInt("SoundOff") == 0;
         private set
         {
            if (value)
            {
               audioMixer.ClearFloat("MasterVol");
            }
            else
            {
               audioMixer.SetFloat("MasterVol", -80.0f);
            }

            OnSoundStateChanged?.Invoke(value);
            PlayerPrefs.SetInt("SoundOff", value ? 0 : 1);
            PlayerPrefs.Save();
         }
      }

      public bool MusicOn
      {
         get => PlayerPrefs.GetInt("MusicOff") == 0;
         private set
         {
            if (value)
            {
               audioMixer.ClearFloat("MusicVol");
            }
            else
            {
               audioMixer.SetFloat("MusicVol", -80.0f);
            }

            OnMusicStateChanged?.Invoke(value);
            PlayerPrefs.SetInt("MusicOff", value ? 0 : 1);
            PlayerPrefs.Save();
         }
      }

      public void SwitchSoundState()
      {
         SoundOn = !SoundOn;
      }

      public void SwitchMusicState()
      {
         MusicOn = !MusicOn;
      }
   }

}