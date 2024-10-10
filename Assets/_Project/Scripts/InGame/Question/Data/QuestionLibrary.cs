using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame.InGame.Question
{
    [CreateAssetMenu(fileName = "QuestionLibrary", menuName = "Scriptable Objects/Question Library")]
    public class QuestionLibrary : ScriptableObject
    {
        [SerializeField] private QuestionParser questionParser;
        public List<QuestionData> questionDataList;

        private Dictionary<QuestionCategory, List<QuestionData>> _categoryToDataAll;
        private Dictionary<QuestionCategory, List<QuestionData>> _categoryToDataNotAskedBefore;


        public QuestionData GetQuestionWithCategory(QuestionCategory category)
        {
            // Check if dictionaries initialized
            if (_categoryToDataAll == null)
            {
                SetupDictionaries();
            }

            // Check if all questions in category has been asked before
            if (_categoryToDataNotAskedBefore[category].Count <= 0)
            {
                RefillCategory(category);
            }

            QuestionData selectedQuestion = _categoryToDataNotAskedBefore[category].RandomElement();

            _categoryToDataNotAskedBefore[category].Remove(selectedQuestion);

            return selectedQuestion;
        }

        void SetupDictionaries()
        {
            _categoryToDataAll = new Dictionary<QuestionCategory, List<QuestionData>>();
            _categoryToDataNotAskedBefore = new Dictionary<QuestionCategory, List<QuestionData>>();

            foreach (QuestionData questionData in questionDataList)
            {
                if (!_categoryToDataAll.ContainsKey(questionData.questionCategory))
                {
                    _categoryToDataAll[questionData.questionCategory] = new List<QuestionData>();
                    _categoryToDataNotAskedBefore[questionData.questionCategory] = new List<QuestionData>();
                }

                _categoryToDataAll[questionData.questionCategory].Add(questionData);
                _categoryToDataNotAskedBefore[questionData.questionCategory].Add(questionData);
            }

        }

        void RefillCategory(QuestionCategory category)
        {
            _categoryToDataNotAskedBefore[category].AddRange(new List<QuestionData>(_categoryToDataAll[category]));
        }

        public void GetDataFromParser()
        {
            if (Application.isPlaying) return;

            questionDataList = questionParser.LoadDataToLibrary();
        }
    }

}