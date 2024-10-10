using System.Collections;
using TriviaGame.Audio;
using TriviaGame.CanvasManagement;
using TriviaGame.InGame.Question;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.InGame.Category
{
    public class CategoryManager : CanvasBase
    {
        [Header("Base")]
        [SerializeField] private Canvas canvas;
        
        [Space(10)]
        [Header("Script References")]
        [SerializeField] private QuestionManager questionManager;
        [SerializeField] private WheelUI wheelUI;
        [SerializeField] private VersusUI versusUI;
        [SerializeField] private SelectedCategoryUI selectedCategoryUI;
        
        [Space(10)] 
        [Header("UI Elements")]
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button categoryWheelButton;
        
        private void OnWheelStop(QuestionCategory selectedCategory)
        {
            AudioManager.instance.PlaySoundFx(SoundType.Success);
            
            // Set selected category and select a question for question menu
            questionManager.SetupNewQuestion(selectedCategory);
            
            // Setup and show selected category menu
            selectedCategoryUI.SetupMenu(selectedCategory);
            selectedCategoryUI.ShowUI();
        }

        private void OnClickCategoryWheel()
        {
            AudioManager.instance.PlaySoundFx(SoundType.Button);
            
            // Disable all interaction while wheel spin
            mainMenuButton.enabled = false;
            categoryWheelButton.enabled = false;
            
            // Spin the wheel
            wheelUI.SpinWheel();
        }

        private void OnClickMainMenu()
        {
            AudioManager.instance.PlaySoundFx(SoundType.Button);
            
            // Disable all interaction and wait for the canvas switch
            mainMenuButton.enabled = false;
            categoryWheelButton.enabled = false;
            
            CanvasManager.instance.SwitchCanvas(CanvasManager.CanvasType.MainMenu);
        }

        private IEnumerator DoShowAnimation(float animDuration)
        {
            // Start show animation in sub menus
            wheelUI.DoMoveInAnimation(animDuration);
            versusUI.DoMoveInAnimation(animDuration);

            // Wait for animations to complete and then activate interactions
            yield return new WaitForSeconds(animDuration);

            categoryWheelButton.enabled = true;
            mainMenuButton.enabled = true;
        }

        public override float ShowCanvas()
        {
            //Disable interactions and wait for animations to complete
            categoryWheelButton.enabled = false;
            mainMenuButton.enabled = false;

            canvas.enabled = true;
            
            // Reset wheel rotation
            wheelUI.ResetWheel();

            float animDuration = 0.75f;

            StartCoroutine(DoShowAnimation(animDuration));

            return animDuration;
        }

        public override float HideCanvas()
        {
            canvas.enabled = false;
            return 0f;
        }

        public override void HideOnStart()
        {
            canvas.enabled = false;
        }


        private void OnEnable()
        {
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
            categoryWheelButton.onClick.AddListener(OnClickCategoryWheel);
            wheelUI.OnWheelStop += OnWheelStop;
        }

        private void OnDisable()
        {
            mainMenuButton.onClick.RemoveListener(OnClickMainMenu);
            categoryWheelButton.onClick.RemoveListener(OnClickCategoryWheel);
            wheelUI.OnWheelStop -= OnWheelStop;
        }
    }

}