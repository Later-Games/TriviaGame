using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TriviaGame.InGame.Question
{
    [CustomEditor(typeof(QuestionLibrary))]
    public class QuestionLibraryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var questionParser = (QuestionLibrary)target;

            if (GUILayout.Button("Parse Data To Library", GUILayout.Height(40)))
            {
                questionParser.GetDataFromParser();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}