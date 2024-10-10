using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TriviaGame.Audio;
using TriviaGame.InGame.Question;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TriviaGame.InGame.Category
{
    public class WheelUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Settings settings;
        
        [Space(10)]
        [Header("UI Elements")]
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private Transform pointerImageTransform;
        [SerializeField] private List<Transform> categoryImages;

        public event Action<QuestionCategory> OnWheelStop;

        public void ResetWheel()
        {
            wheelTransform.transform.localRotation = Quaternion.identity;
        }

        public void DoMoveInAnimation(float animDuration)
        {
            Vector3 wheelPosition = transform.localPosition;
            transform.localPosition = wheelPosition - Vector3.up * Screen.height;

            transform.DOLocalMove(wheelPosition, animDuration).SetEase(Ease.OutBack);
        }

        public void SpinWheel()
        {
            StartCoroutine(StartSpinning());
        }

        IEnumerator StartSpinning()
        {
            wheelTransform.transform.localRotation = Quaternion.identity;

            float spinStartTime = Time.time;
            float pointerTriggerAngle = 360f / settings.categorySettings.Count;

            // Randomize duration and speed to get random rotation
            float randomizedDuration = settings.spinDuration + Random.Range(-1.5f, 1.5f);
            float randomizedSpeed = settings.spinSpeed + Random.Range(-150f, 150f);

            while (Time.time - spinStartTime < randomizedDuration)
            {
                // Calculate and rotate wheel with animationCurve
                float timeRatio = (Time.time - spinStartTime) / randomizedDuration;
                float degrees = settings.animationCurve.Evaluate(timeRatio) * randomizedSpeed;
                if (Mathf.Abs(degrees) > 0)
                {
                    wheelTransform.Rotate(0, 0, degrees * Time.deltaTime, Space.Self);
                }

                // Decrease pointer trigger angle
                pointerTriggerAngle += degrees * Time.deltaTime;

                // Tilt pointer if reached 0 and reset
                if (pointerTriggerAngle < 0)
                {
                    AudioManager.instance.PlaySoundFx(SoundType.WheelTick);

                    pointerTriggerAngle += (360f / settings.categorySettings.Count);

                    pointerImageTransform.DORotate(Vector3.back * 10, 0.1f)
                        .SetLoops(2, LoopType.Yoyo)
                        .From(Vector3.zero);
                }

                yield return null;
            }

            AudioManager.instance.PlaySoundFx(SoundType.Yay);
            DetermineCategory();
        }

        private void DetermineCategory()
        {
            //parse wheel rotation into category
            QuestionCategory category = ParseRotationToCategory();

            // Do scale animation to selected category image
            categoryImages[(int)category].DOScale(Vector3.one * 1.25f, 0.25f).SetLoops(2, LoopType.Yoyo);
            categoryImages[(int)category].DOScale(Vector3.one * 1.15f, 0.15f)
                .SetDelay(0.5f)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => { OnWheelStop?.Invoke(category); });
        }

        private QuestionCategory ParseRotationToCategory()
        {
            //Determine category according to current wheel angle
            QuestionCategory selectedCategory = QuestionCategory.GeneralCulture;

            float wheelRotation = wheelTransform.rotation.eulerAngles.z;

            float rotationPerCategory = 360f / settings.categorySettings.Count;

            for (int i = 0; i < settings.categorySettings.Count; i++)
            {
                if (wheelRotation > i * rotationPerCategory && wheelRotation < (i + 1) * rotationPerCategory)
                {
                    selectedCategory = (QuestionCategory)i;
                    break;
                }
            }

            return selectedCategory;
        }
    }

}