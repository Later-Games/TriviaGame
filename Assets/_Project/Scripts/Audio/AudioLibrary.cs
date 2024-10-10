using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame.Audio
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        public List<AudioData> audioDataList;
        public List<AudioData> musicDataList;

    }
}