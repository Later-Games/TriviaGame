using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.UI.Leaderboard
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private Image profileImage;
        [SerializeField] private TMP_Text nicknameText;
        [SerializeField] private TMP_Text rankText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private RectTransform rectTransform;

        public void SetData(LeaderboardUserData leaderboardUserData, Sprite profileSprite)
        {
            nicknameText.text = leaderboardUserData.nickname;
            rankText.text = leaderboardUserData.rank.ToString();
            scoreText.text = leaderboardUserData.score.ToString();
            profileImage.sprite = profileSprite;
            gameObject.SetActive(true);
        }

        public void SetPosition(Vector3 targetPosition)
        {
            rectTransform.anchoredPosition = targetPosition;
        }
        public float GetHeight()
        {
            return rectTransform.sizeDelta.y;
        }
    }

}