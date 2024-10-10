using System;

namespace TriviaGame.InGame.Question
{
    [Serializable]
    public class QuestionData
    {
        public string question;
        public QuestionCategory questionCategory;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public enum QuestionCategory
    {
        GeneralCulture,
        History,
        Music,
        Cinema
    }

}