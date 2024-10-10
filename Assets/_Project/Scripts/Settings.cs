using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriviaGame.Audio;
using TriviaGame.InGame.Category;
using TriviaGame.InGame.Question;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Setting")]
public class Settings : ScriptableObject
{
    [Header("Leaderboard Setting")] 
    public string dataApiUrlBase = "https://magegamessite.web.app/case1/";

    [Header("Question Setting")]
    public QuestionLibrary questionLibrary;
    public float questionTimer;
    public int correctAnswerPoints;
    public int wrongAnswerPoints;
    public int timeoutPoints;
    public List<CategorySettings> categorySettings;
    [Space(10)] 
    
    [Header("Audio Setting")]
    public AudioLibrary audioLibrary;
    [Space(10)] 

    [Header("Player Settings")]
    public List<Sprite> profileSprites;

    [Header("Wheel Settings")]
    public float spinSpeed = -1000f;
    public float spinDuration = 5f;
    public AnimationCurve animationCurve;
    
    //public accessor to category settings with QuestionCategory
    private Dictionary<QuestionCategory, CategorySettings> _categoryToSettings;
    public CategorySettings GetSettingsForCategory(QuestionCategory category)
    {
        if (_categoryToSettings == null)
        {
            _categoryToSettings = new Dictionary<QuestionCategory, CategorySettings>(
                categorySettings.ToDictionary(x=>x.category, x=>x)
                );
        }

        return _categoryToSettings[category];
    }
}
