using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TriviaGame.InGame.Category
{
    public class VersusUI : MonoBehaviour
    {
        [SerializeField] private Transform playerLineTransform;
        [SerializeField] private Transform enemyLineTransform;

        public void DoMoveInAnimation(float animDuration)
        {
            float playerLinePositionX = playerLineTransform.localPosition.x;
            float enemyLinePositionX = enemyLineTransform.localPosition.x;

            playerLineTransform.localPosition -= Vector3.right * Screen.width;
            enemyLineTransform.localPosition += Vector3.right * Screen.width;

            playerLineTransform.DOLocalMoveX(playerLinePositionX, animDuration);
            enemyLineTransform.DOLocalMoveX(enemyLinePositionX, animDuration);
        }
    }

}