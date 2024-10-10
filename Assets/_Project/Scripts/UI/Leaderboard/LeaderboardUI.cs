using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace TriviaGame.UI.Leaderboard
{
    public class LeaderboardUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Settings settings;
        
        [Space(10)] 
        [Header("Script References")]
        [SerializeField] private ScoreElement scoreElementPrefab;
        
        [Space(10)]
        [Header("UI Elements")]
        [SerializeField] private RectTransform scoreContainer;
        [SerializeField] private ScrollRect scoreScrollRect;

        private List<ScoreElement> _scoreElements = new List<ScoreElement>();

        public void BuildLeaderboard(LeaderboardData leaderboardData)
        {
            Vector3 targetPosition = new Vector3(0, -100, 0);
            float distanceBetween = 25;
            
            for (int i = 0; i < leaderboardData.data.Length; i++)
            {
                if (_scoreElements.Count <= i)
                {
                    ScoreElement element = Instantiate(scoreElementPrefab, scoreContainer);
                    _scoreElements.Add(element);
                }
                
                _scoreElements[i].SetData(leaderboardData.data[i], 
                    settings.profileSprites[i % settings.profileSprites.Count]);

                _scoreElements[i].SetPosition(targetPosition);

                targetPosition.y -= _scoreElements[i].GetHeight() + distanceBetween;
            }

            //İf any page has less elements than previous, disable extra elements
            DisableExtraElements(leaderboardData.data.Length);
                
            Vector2 containerSize = scoreContainer.sizeDelta;
            containerSize.y = Mathf.Abs(targetPosition.y) - 50;
            scoreContainer.sizeDelta = containerSize;
        }

        private void DisableExtraElements(int targetElementCount)
        {
            if(_scoreElements.Count <= targetElementCount) return;

            int startIndex = _scoreElements.Count - (_scoreElements.Count - targetElementCount);
            for (int i = startIndex; i < _scoreElements.Count; i++)
            {
                _scoreElements[i].gameObject.SetActive(false);
            }
        }

        public void ResetScrollPosition()
        {
            SetScrollState(false);
            var position = scoreContainer.anchoredPosition;
            position.y = 0;
            scoreContainer.anchoredPosition = position;
            SetScrollState(true);
        }

        public void SetScrollState(bool state)
        {
            // enable and disable scroll in animations and to make sure it is not flicker
            scoreScrollRect.enabled = state;
        }
        
    }

}