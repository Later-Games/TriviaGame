using System.Collections;
using TriviaGame.CanvasManagement;
using UnityEngine;

namespace TriviaGame.InGame.Question
{
    public class QuestionManager : CanvasBase
    {
        [Header("Base")]
        [SerializeField] private Canvas canvas;
        
        [Space(10)] 
        [Header("Data")]
        [SerializeField] private Settings settings;
        
        [Space(10)] 
        [Header("Script References")]
        [SerializeField] private QuestionUI questionUI;
        [SerializeField] private NextMenuUI nextMenuUI;
        
        public void SetupNewQuestion(QuestionCategory questionCategory)
        {
            // Select a question with data
            QuestionData questionData = settings.questionLibrary.GetQuestionWithCategory(questionCategory);

            // Feed question data to question UI
            questionUI.SetupQuestion(questionData, settings.questionTimer);
        }

        void OnAnswerAnimationComplete(QuestionUI.AnswerType answerType)
        {
            // Select target menu =>
            // if answer is correct then move to category section and continue playing with new question
            // if answer is not correct then you failed, move back to main menu

            CanvasManager.CanvasType targetMenu = answerType == QuestionUI.AnswerType.Correct
                ? CanvasManager.CanvasType.Category
                : CanvasManager.CanvasType.MainMenu;

            // Setup next menu target
            nextMenuUI.DoMoveInAnimation(targetMenu, 0.3f);
        }

        IEnumerator DoShowAnimation(float animationDuration)
        {
            questionUI.DoMoveInAnimation(animationDuration);

            yield return new WaitForSeconds(animationDuration);

            questionUI.StartQuestion();
        }

        public override float ShowCanvas()
        {
            canvas.enabled = true;

            StartCoroutine(DoShowAnimation(0.5f));
            return 0.5f;
        }

        public override float HideCanvas()
        {
            canvas.enabled = false;
            return 0f;
        }

        public override void HideOnStart()
        {
            canvas.enabled = false;
        }


        private void OnEnable()
        {
            questionUI.OnAnswerAnimationComplete += OnAnswerAnimationComplete;
        }

        private void OnDisable()
        {
            questionUI.OnAnswerAnimationComplete -= OnAnswerAnimationComplete;
        }
    }
}