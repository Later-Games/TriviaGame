using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace TriviaGame.UI
{
    public class PointsManager : MonoBehaviour
    {
        [SerializeField] private Transform pointsImageTransform;
        [SerializeField] private Transform losePointsImageTransform;

        private int _points;
        public int Points => PlayerPrefs.GetInt("Points-", 0);

        public event Action<int> PointsUpdated;

        public static PointsManager instance;

        private IObjectPool<Transform> _pointsImagePool;
        private IObjectPool<Transform> _losePointsImagePool;
        
        private void Awake()
        {
            // Singleton
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            _pointsImagePool = new ObjectPool<Transform>(CreatePointsImage, OnGet, OnRelease);
            _losePointsImagePool = new ObjectPool<Transform>(CreateLosePointsImage, OnGet, OnRelease);
        }

        private void Start()
        {
            _points = PlayerPrefs.GetInt("Points-", 0);
            PointsUpdated?.Invoke(_points);
        }

        public void EarnPoints(int amount)
        {
            _points += amount;

            PlayerPrefs.SetInt("Points-", _points);

            PointsUpdated?.Invoke(_points);
        }

        public void LosePoints(int amount)
        {
            _points = Mathf.Max(0, _points - amount);

            PlayerPrefs.SetInt("Points-", _points);

            PointsUpdated?.Invoke(_points);
        }

        public void DoLosePointsAnimation(int pointCount)
        {
            int iterationCount = CalculateCounts(pointCount, 20, out int pointsPerIteration, out int remainder);

            for (int i = 0; i < iterationCount; i++)
            {
                Transform newPointImage = _losePointsImagePool.Get();
                newPointImage.position = losePointsImageTransform.position;
                newPointImage.localScale = Vector3.zero;

                float delay = i * Mathf.Min(0.25f, 2f / iterationCount);
                int index = i;

                Vector3 targetPosition = newPointImage.position + Vector3.down * 100;


                newPointImage.DOScale(Vector3.one * 0.5f, 0f).SetDelay(delay)
                    .OnStart(() => { LosePoints(index == iterationCount - 1 ? remainder : pointsPerIteration); });
                newPointImage.DOMove(targetPosition, 0.5f).SetDelay(delay);
                newPointImage.DOScale(Vector3.one * 1.25f, 0.25f).SetDelay(delay).SetLoops(2, LoopType.Yoyo);
                newPointImage.DOScale(Vector3.zero, 0.45f).SetDelay(0.5f + delay)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => { _losePointsImagePool.Release(newPointImage); });
            }
        }

        public void DoEarnPointsAnimation(int pointCount, Vector3 position)
        {
            float randomPositionMaxMin = 200f;

            int iterationCount = CalculateCounts(pointCount, 15, out int pointsPerIteration, out int remainder);

            for (int i = 0; i < iterationCount; i++)
            {
                Transform newPointImage = _pointsImagePool.Get();
                newPointImage.position = position + new Vector3(
                    Random.Range(-randomPositionMaxMin, randomPositionMaxMin),
                    Random.Range(-randomPositionMaxMin, randomPositionMaxMin), 0);

                newPointImage.localScale = Vector3.zero;

                float delay = i * 0.05f;
                int index = i;

                newPointImage.DOScale(Vector3.one * 1.25f, 0.25f).SetDelay(delay).SetEase(Ease.OutBack);
                newPointImage.DOMove(pointsImageTransform.position, 0.75f).SetDelay(0.35f + delay).OnComplete(() =>
                {
                    EarnPoints(index == iterationCount - 1 ? remainder : pointsPerIteration);
                    _pointsImagePool.Release(newPointImage);
                });
            }
        }

        private int CalculateCounts(int pointCount, int maxIterationCount, out int pointsPerIteration,
            out int remainder)
        {
            int iterationCount = Mathf.Min(maxIterationCount, pointCount);
            pointsPerIteration = 1;
            remainder = 1;

            if (pointCount > iterationCount)
            {
                pointsPerIteration = pointCount / iterationCount;
                remainder = pointCount % (pointsPerIteration * iterationCount);
                if (remainder == 0)
                {
                    remainder = pointsPerIteration;
                }
                else
                {
                    iterationCount++;
                }
            }

            return iterationCount;
        }

        private Transform CreatePointsImage()
        {
            return Instantiate(pointsImageTransform, pointsImageTransform.parent);
        }
        
        private Transform CreateLosePointsImage()
        {
            return Instantiate(losePointsImageTransform, pointsImageTransform.parent);
        }

        private void OnRelease(Transform obj)
        {
            obj.gameObject.SetActive(false);
        }
        
        private void OnGet(Transform obj)
        {
            obj.gameObject.SetActive(true);
        }
    }

}
