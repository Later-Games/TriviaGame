using DG.Tweening;
using TMPro;
using TriviaGame.Audio;
using TriviaGame.CanvasManagement;
using TriviaGame.InGame.Question;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.InGame.Category
{
    public class SelectedCategoryUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private Settings settings;
        
        [Space(10)]
        [Header("UI Elements")]
        [SerializeField] private Transform menuTransform;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text characterNameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private Image menuBackground;
        [SerializeField] private Image backgroundBlock;
        [SerializeField] private Button playButton;

        public void SetupMenu(QuestionCategory questionCategory)
        {
            CategorySettings categorySettings = settings.GetSettingsForCategory(questionCategory);

            titleText.text = categorySettings.categoryName;
            characterNameText.text = categorySettings.characterName;
            characterImage.sprite = categorySettings.characterSprite;
            menuBackground.color = categorySettings.categoryColor;
            playButton.image.color = categorySettings.categoryColor;
        }

        public void ShowUI()
        {
            menuTransform.localScale = Vector3.zero;
            playButton.enabled = false;

            gameObject.SetActive(true);
            backgroundBlock.gameObject.SetActive(true);
            backgroundBlock.DOFade(0.9f, 0.5f).From(0);
            menuTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                playButton.enabled = true;
            });
        }

        private void HideUI()
        {
            AudioManager.instance.PlaySoundFx(SoundType.Button);

            playButton.enabled = false;

            backgroundBlock.DOFade(0f, 0.5f);
            menuTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                CanvasManager.instance.SwitchCanvas(CanvasManager.CanvasType.Question);
                gameObject.SetActive(false);
                backgroundBlock.gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            playButton.onClick.AddListener(HideUI);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(HideUI);
        }
    }
}