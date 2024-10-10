using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace TriviaGame.InGame.Question
{
    [CreateAssetMenu(fileName = "QuestionParser", menuName = "Scriptable Objects/Question Parser")]
    public class QuestionParser : ScriptableObject
    {
        // Drag and drop your JSON file here in the Inspector
        public TextAsset jsonFile;

        public List<QuestionData> LoadDataToLibrary()
        {
            List<QuestionData> questionsDataList = new List<QuestionData>();

            if (jsonFile != null)
            {
                try
                {
                    // Read the JSON data from the TextAsset and remove line breaks
                    string jsonData = jsonFile.text.Replace("\n", "").Replace("\r", "");

                    // Deserialize JSON into a temporary data structure
                    JsonQuestionList jsonQuestionList = JsonConvert.DeserializeObject<JsonQuestionList>(jsonData);

                    // Convert JSON questions to QuestionData
                    foreach (var jsonQuestion in jsonQuestionList.questions)
                    {
                        QuestionData questionData = new QuestionData
                        {
                            question = jsonQuestion.question,
                            questionCategory = ParseCategory(jsonQuestion.category),
                            answers = jsonQuestion.choices,
                            correctAnswerIndex = ParseAnswer(jsonQuestion.answer)
                        };

                        questionsDataList.Add(questionData);
                    }

                    Debug.Log("Questions successfully loaded and parsed.");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing JSON file: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError("JSON file is not assigned in the inspector.");
            }

            return questionsDataList;
        }

        private QuestionCategory ParseCategory(string category)
        {
            switch (category.ToLower())
            {
                case "general-culture":
                    return QuestionCategory.GeneralCulture;
                case "history":
                    return QuestionCategory.History;
                case "music":
                    return QuestionCategory.Music;
                case "cinema":
                    return QuestionCategory.Cinema;
                default:
                    Debug.LogWarning($"Unknown category: {category}, defaulting to GeneralCulture.");
                    return QuestionCategory.GeneralCulture;
            }
        }

        // Convert answer to correct index
        private int ParseAnswer(char answer)
        {
            return (char.ToUpper(answer) - 'A');
        }
    }

    [Serializable]
    public class JsonQuestion
    {
        public string category;
        public string question;
        public string[] choices;
        public char answer;
    }

    [Serializable]
    public class JsonQuestionList
    {
        public List<JsonQuestion> questions;
    }
}