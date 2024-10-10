using DG.Tweening;
using TriviaGame.Audio;
using TriviaGame.CanvasManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.InGame.Question
{
    public class NextMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button likeButton;
        [SerializeField] private Button dislikeButton;

        private float _likeButtonPosition;
        private float _dislikeButtonPosition;

        private CanvasManager.CanvasType _nextCanvasType;

        private void Start()
        {
            _likeButtonPosition = likeButton.transform.localPosition.x;
            _dislikeButtonPosition = dislikeButton.transform.localPosition.x;
            Hide();
        }

        public void Hide()
        {
            SetButtonState(false);

            Vector3 continueButtonPosition = continueButton.transform.localPosition;
            continueButtonPosition.x = Screen.width;
            continueButton.transform.localPosition = continueButtonPosition;

            Vector3 likeButtonPosition = likeButton.transform.localPosition;
            likeButtonPosition.x = _likeButtonPosition + Screen.width;
            likeButton.transform.localPosition = likeButtonPosition;

            Vector3 dislikeButtonPosition = dislikeButton.transform.localPosition;
            dislikeButtonPosition.x = _dislikeButtonPosition - Screen.width;
            dislikeButton.transform.localPosition = dislikeButtonPosition;

            gameObject.SetActive(false);
        }

        public void DoMoveInAnimation(CanvasManager.CanvasType nextCanvasType, float moveDuration)
        {
            gameObject.SetActive(true);

            // Setup next menu (Main Menu if failed, Category Menu if succeeded)
            _nextCanvasType = nextCanvasType;
            
            // Disable buttons for animation
            SetButtonState(false);

            //Move button in with animation
            continueButton.transform.DOLocalMoveX(0f, moveDuration);
            dislikeButton.transform.DOLocalMoveX(_dislikeButtonPosition, moveDuration).SetDelay(0.1f);
            likeButton.transform.DOLocalMoveX(_likeButtonPosition, moveDuration).SetDelay(0.1f)
                .OnComplete(() => { SetButtonState(true); });
        }

        void SetButtonState(bool state)
        {
            continueButton.enabled = state;
            likeButton.enabled = state;
            dislikeButton.enabled = state;
        }

        void OnClickContinueButton()
        {
            Hide();

            AudioManager.instance.PlaySoundFx(SoundType.Button);
            CanvasManager.instance.SwitchCanvas(_nextCanvasType);
        }

        void OnClickLikeButton()
        {
            // Send feedback data
            Hide();

            AudioManager.instance.PlaySoundFx(SoundType.Button);
            CanvasManager.instance.SwitchCanvas(_nextCanvasType);
        }

        void OnClickDislikeButton()
        {
            // Send feedback data
            Hide();

            AudioManager.instance.PlaySoundFx(SoundType.Button);
            CanvasManager.instance.SwitchCanvas(_nextCanvasType);
        }

        private void OnEnable()
        {
            continueButton.onClick.AddListener(OnClickContinueButton);
            likeButton.onClick.AddListener(OnClickLikeButton);
            dislikeButton.onClick.AddListener(OnClickDislikeButton);
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(OnClickContinueButton);
            likeButton.onClick.RemoveListener(OnClickLikeButton);
            dislikeButton.onClick.RemoveListener(OnClickDislikeButton);
        }
    }
}