using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using TriviaGame.Audio;
using TriviaGame.InGame.Category;
using TriviaGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.InGame.Question
{
    public class QuestionUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Settings settings;
        
        [Space(10)] 
        [Header("Script References")]
        [SerializeField] private TimerUI timerUI;
        [SerializeField] private Choice[] choices;

        [Space(10)] 
        [Header("UI Elements")]
        [SerializeField] private Transform questionParent;
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private Image titleBackground;

        public event Action<AnswerType> OnAnswerAnimationComplete;

        private bool _questionCompleted = true;

        private float _startTime;
        private float _giveTotalTime;

        private int _correctAnswerIndex;

        private void Update()
        {
            // if question already answered or timeout do nothing and return
            if (_questionCompleted) return;

            // Calculate remaining time, make sure not to go below zero
            float remainingTime = Mathf.Max(0, _giveTotalTime - (Time.time - _startTime));

            // Raise timeout flag if time is over
            if (remainingTime <= 0)
            {
                _questionCompleted = true;
                OnQuestionAnswer(AnswerType.Timeout);
            }

            // Update timer UI with remaning time, and percentage of it
            timerUI.UpdateTimerUI(remainingTime, remainingTime / _giveTotalTime);
        }

        public void SetupQuestion(QuestionData questionData, float givenTotalTime)
        {
            // Update UI with data
            _giveTotalTime = givenTotalTime;
            questionText.text = questionData.question;
            _correctAnswerIndex = questionData.correctAnswerIndex;

            // Setup background colors according to question category
            CategorySettings categorySettings = settings.GetSettingsForCategory(questionData.questionCategory);
            titleBackground.sprite = categorySettings.titleBackground;

            timerUI.ResetTimer();

            for (int i = 0; i < 4; i++)
            {
                choices[i].SetupOption(questionData.answers[i], i == _correctAnswerIndex);
            }
        }

        public void DoMoveInAnimation(float animationDuration)
        {
            var titlePosition = titleBackground.transform.localPosition;

            titleBackground.transform.localPosition += Vector3.up * Screen.height;
            titleBackground.transform.DOLocalMove(titlePosition, animationDuration);
            questionParent.DOScale(Vector3.one, animationDuration).From(Vector3.zero);
            DoMoveChoicesInAnimation(animationDuration);
            timerUI.DoMoveInAnimation(animationDuration);
        }

        public void StartQuestion()
        {
            _startTime = Time.time;
            _questionCompleted = false;

            SetChoiceStates(true);
        }

        private IEnumerator DoAnswerAnimations(AnswerType answerType)
        {
            _questionCompleted = true;

            float animationDuration = 0f;
            switch (answerType)
            {
                case AnswerType.Correct:
                    DoCorrectAnswerAnimation();
                    animationDuration = 0.5f;
                    break;
                case AnswerType.Wrong:
                    DoWrongAnswerAnimation();
                    animationDuration = 0.5f;
                    break;
                case AnswerType.Timeout:
                    DoTimeoutAnimation();
                    break;
            }

            yield return new WaitForSeconds(animationDuration);

            float duration = DoMoveChoicesOutAnimation();

            yield return new WaitForSeconds(duration);

            OnAnswerAnimationComplete?.Invoke(answerType);
        }

        private void OnQuestionAnswer(AnswerType answerType)
        {
            StartCoroutine(DoAnswerAnimations(answerType));
            AudioManager.instance.StopSoundFx(SoundType.Clock);
        }

        private void DoCorrectAnswerAnimation()
        {
            SetChoiceStates(false);

            Vector3 correctButtonPosition = choices[_correctAnswerIndex].transform.position;
            PointsManager.instance.DoEarnPointsAnimation(settings.correctAnswerPoints, correctButtonPosition);

            AudioManager.instance.PlaySoundFx(SoundType.Correct);
            choices[_correctAnswerIndex].DoResultAnimation();
        }

        private void DoWrongAnswerAnimation()
        {
            SetChoiceStates(false);

            PointsManager.instance.DoLosePointsAnimation(settings.wrongAnswerPoints);

            AudioManager.instance.PlaySoundFx(SoundType.Wrong);
            choices[_correctAnswerIndex].DoResultAnimation();
        }

        private void DoTimeoutAnimation()
        {
            SetChoiceStates(false);

            AudioManager.instance.PlaySoundFx(SoundType.Fail);
            PointsManager.instance.DoLosePointsAnimation(settings.timeoutPoints);
        }

        private float DoMoveChoicesOutAnimation()
        {
            float delay = 0.075f;
            float moveDuration = 0.3f;
            for (int i = 0; i < 4; i++)
            {
                choices[i].DoMoveOutAnimation(moveDuration, i * delay);
            }

            return 3 * delay + moveDuration;
        }

        private void DoMoveChoicesInAnimation(float duration)
        {
            float delay = 0.03f;

            for (int i = 0; i < 4; i++)
            {
                choices[i].DoMoveInAnimation(duration - delay * 3f, i * delay);
            }
        }

        private void SetChoiceStates(bool state)
        {
            for (int i = 0; i < 4; i++)
            {
                choices[i].SetState(state);
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < 4; i++)
            {
                choices[i].OnResponse += OnQuestionAnswer;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < 4; i++)
            {
                choices[i].OnResponse -= OnQuestionAnswer;
            }
        }

        public enum AnswerType
        {
            Correct,
            Wrong,
            Timeout
        }

    }
}