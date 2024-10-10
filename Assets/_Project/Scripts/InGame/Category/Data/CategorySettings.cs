using System.Collections;
using System.Collections.Generic;
using TriviaGame.InGame.Question;
using UnityEngine;

namespace TriviaGame.InGame.Category
{
    [CreateAssetMenu(fileName = "CategorySettings", menuName = "Scriptable Objects/Category Setting")]
    public class CategorySettings : ScriptableObject
    {
        public QuestionCategory category;
        public string categoryName;
        public Color categoryColor;
        public string characterName;
        public Sprite characterSprite;
        public Sprite titleBackground;
    }
}