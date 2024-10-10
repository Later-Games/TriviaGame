using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TriviaGame.Audio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TriviaGame.InGame.Question
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image fillImage;
        [SerializeField] private float alarmStartTime;
        [SerializeField] private float alarmRotateAmount;
        [SerializeField] private int rotPerSecond;


        private bool _isShaking;
        private Quaternion _startRotation;

        private void Start()
        {
            _startRotation = transform.rotation;
        }

        public void DoMoveInAnimation(float animDuration)
        {
            transform.DOScale(Vector3.one, animDuration).From(Vector3.zero);
        }

        public void UpdateTimerUI(float remainingTime, float timePercentage)
        {
            // Round and set remaining time
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();

            // Set fill amount according to percentage
            fillImage.fillAmount = timePercentage;

            // Check if time is in alarm rate and start shaking timer image
            if (remainingTime <= alarmStartTime && !_isShaking && remainingTime > 0)
            {
                AudioManager.instance.PlaySoundFx(SoundType.Clock);
                
                _isShaking = true;
                
                // Do Shake Animation
                int rotationCount = Mathf.FloorToInt(remainingTime) * rotPerSecond;
                float rotationDuration = remainingTime / rotationCount;
                transform.DORotate(_startRotation.eulerAngles + Vector3.forward * alarmRotateAmount, rotationDuration)
                    .SetLoops(rotationCount, LoopType.Yoyo);
            }
        }

        public void ResetTimer()
        {
            fillImage.fillAmount = 1f;
            _isShaking = false;
            transform.DOKill();
            transform.rotation = _startRotation;
        }
    }

}