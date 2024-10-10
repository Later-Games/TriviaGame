using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TriviaGame.UI
{
    public class PointsUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private RectTransform pointsImageTransform;

        private void Awake()
        {
            PointsManager.instance.PointsUpdated += PointsUpdated;
        }

        void PointsUpdated(int pointCount)
        {
            pointsText.text = pointCount.ToString();

            pointsImageTransform.DOKill();
            var pos = pointsImageTransform.anchoredPosition;
            pos.y = 0f;
            pointsImageTransform.anchoredPosition = pos;

            pointsImageTransform.DOAnchorPosY(20f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }

        public void DoHideAnimation()
        {
            transform.DOLocalMoveX(transform.localPosition.x + Screen.width, 0.5f);
        }

        public void DoShowAnimation()
        {
            transform.DOLocalMoveX(transform.localPosition.x - Screen.width, 0.5f);
        }
    }
}