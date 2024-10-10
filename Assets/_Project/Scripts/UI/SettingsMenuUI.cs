using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TriviaGame.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.UI
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        [Space(10)]
        [Header("Script References")]
        [SerializeField] private AudioManager audioManager;
        
        [Space(10)] 
        [Header("UI Elements")]
        [SerializeField] private Image backgroundCover;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button openButton;
        
        [Space(5)] 
        [Header("Sound")] 
        [SerializeField] private Button soundButton;
        [SerializeField] private Transform soundToggle;
        [SerializeField] private Image soundToggleBg;
        
        [Space(5)] [Header("Music")] 
        [SerializeField] private Button musicButton;
        [SerializeField] private Transform musicToggle;
        [SerializeField] private Image musicToggleBg;
        
        [Space(10)] 
        [Header("Settings")]
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;
        

        private float _onXPosition = 45f;
        private float _offXPosition = -45f;

        private void Start()
        {
            HideSettingsMenu();
        }

        public void ShowMenu()
        {
            SetSoundToggleState(AudioManager.instance.SoundOn);
            SetMusicToggleState(AudioManager.instance.MusicOn);

            AudioManager.instance.PlaySoundFx(SoundType.Button);

            backgroundCover.DOFade(0.95f, 0.5f).From(0);

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            closeButton.enabled = true;
            canvas.enabled = true;
        }

        public void HideSettingsMenu()
        {
            closeButton.enabled = false;

            AudioManager.instance.PlaySoundFx(SoundType.Button);

            backgroundCover.DOFade(0f, 0.23f).From(0.95f);

            transform.DOScale(Vector3.zero, 0.23f).SetEase(Ease.InBack).OnComplete(() => { canvas.enabled = false; });
        }

        public void SetSoundToggleState(bool state)
        {
            if (state)
            {
                soundToggleBg.color = onColor;
                soundToggle.transform.DOLocalMoveX(_onXPosition, 0.25f);
            }
            else
            {
                soundToggleBg.color = offColor;
                soundToggle.transform.DOLocalMoveX(_offXPosition, 0.25f);
            }
        }

        public void SetMusicToggleState(bool state)
        {
            if (state)
            {
                musicToggleBg.color = onColor;
                musicToggle.transform.DOLocalMoveX(_onXPosition, 0.25f);
            }
            else
            {
                musicToggleBg.color = offColor;
                musicToggle.transform.DOLocalMoveX(_offXPosition, 0.25f);
            }
        }

        private void OnClickSoundButton()
        {
            AudioManager.instance.SwitchSoundState();
            AudioManager.instance.PlaySoundFx(SoundType.Button);
        }

        private void OnClickMusicButton()
        {
            AudioManager.instance.SwitchMusicState();
            AudioManager.instance.PlaySoundFx(SoundType.Button);
        }

        private void OnEnable()
        {
            soundButton.onClick.AddListener(OnClickSoundButton);
            musicButton.onClick.AddListener(OnClickMusicButton);
            closeButton.onClick.AddListener(HideSettingsMenu);
            openButton.onClick.AddListener(ShowMenu);

            audioManager.OnSoundStateChanged += SetSoundToggleState;
            audioManager.OnMusicStateChanged += SetMusicToggleState;
        }

        private void OnDisable()
        {
            soundButton.onClick.RemoveListener(OnClickSoundButton);
            musicButton.onClick.RemoveListener(OnClickMusicButton);
            closeButton.onClick.RemoveListener(HideSettingsMenu);
            openButton.onClick.RemoveListener(ShowMenu);

            audioManager.OnSoundStateChanged -= SetSoundToggleState;
            audioManager.OnMusicStateChanged -= SetMusicToggleState;
        }
    }
}