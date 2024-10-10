using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.InGame.Question
{
    public class Choice : MonoBehaviour
    {
        [SerializeField] private Button answerButton;
        [SerializeField] private TMP_Text answerText;

        public event Action<QuestionUI.AnswerType> OnResponse;

        private bool _isCorrect;


        public void SetupOption(string answer, bool isCorrect)
        {
            _isCorrect = isCorrect;
            answerText.text = answer;

            var defaultPosition = transform.localPosition;
            defaultPosition.x = 0;
            transform.localPosition = defaultPosition;
        }

        public void SetState(bool state)
        {
            answerButton.enabled = state;
        }

        public void DoResultAnimation()
        {
            answerButton.transform.DOScale(Vector3.one * 1.05f, 0.1f).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => { answerButton.image.color = _isCorrect ? Color.green : Color.red; });
        }

        public void DoMoveOutAnimation(float moveDuration, float delay)
        {
            transform.DOLocalMoveX(-Screen.width, moveDuration)
                .SetEase(Ease.InBack)
                .SetDelay(delay)
                .OnComplete(() => { answerButton.image.color = Color.white; });
        }

        public void DoMoveInAnimation(float moveDuration, float delay)
        {
            transform.DOLocalMoveX(0f, moveDuration).From(Screen.width)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .OnComplete(() => { answerButton.image.color = Color.white; });
        }

        private void OnEnable()
        {
            answerButton.onClick.AddListener(OnClickedButton);
        }

        private void OnDisable()
        {
            answerButton.onClick.RemoveListener(OnClickedButton);
        }

        private void OnClickedButton()
        {
            if (_isCorrect)
            {
                OnResponse?.Invoke(QuestionUI.AnswerType.Correct);
            }
            else
            {
                OnResponse?.Invoke(QuestionUI.AnswerType.Wrong);
                DoResultAnimation();
            }
        }
    }
}