using TriviaGame.Audio;
using TriviaGame.CanvasManagement;
using TriviaGame.UI.Leaderboard;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.UI
{
    public class MainMenuUI : CanvasBase
    {
        [Header("Base")]
        [SerializeField] private Canvas canvas;
        
        [Space(10)] 
        [Header("UI Elements")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button leaderboardButton;

        private void OnClickPlayButton()
        {
            AudioManager.instance.PlaySoundFx(SoundType.Button);
            CanvasManager.instance.SwitchCanvas(CanvasManager.CanvasType.Category);
        }

        private void OnClickLeaderboardButton()
        {
            AudioManager.instance.PlaySoundFx(SoundType.Button);
            CanvasManager.instance.SwitchCanvas(CanvasManager.CanvasType.Leaderboard);
        }


        public override float ShowCanvas()
        {
            canvas.enabled = true;
            return 0f;
        }

        public override float HideCanvas()
        {
            canvas.enabled = false;
            return 0f;
        }

        public override void HideOnStart()
        {
            // Dont hide main manu on start
        }


        private void OnEnable()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            leaderboardButton.onClick.AddListener(OnClickLeaderboardButton);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnClickPlayButton);
            leaderboardButton.onClick.RemoveListener(OnClickLeaderboardButton);
        }
    }

}