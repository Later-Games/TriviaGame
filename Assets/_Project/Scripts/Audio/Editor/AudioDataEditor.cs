using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TriviaGame.Audio
{
    [CustomEditor(typeof(AudioData), true)]
    public class AudioDataEditor : Editor
    {
        [SerializeField] AudioSource previewer;

        public void OnEnable()
        {
            previewer = EditorUtility
                .CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource))
                .GetComponent<AudioSource>();
        }

        public void OnDisable()
        {
            DestroyImmediate(previewer.gameObject);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Preview"))
            {
                ((AudioData)target).Play(previewer);
            }

            EditorGUI.EndDisabledGroup();
        }
    }

}