using System.Collections;
using System.Collections.Generic;
using TriviaGame.Audio;
using DG.Tweening;
using TriviaGame.CanvasManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.UI.Leaderboard
{
    public class LeaderboardManager : CanvasBase
    {
        [Header("Base")]
        [SerializeField] private Canvas canvas;
        
        [Space(10)] 
        [Header("Script References")]
        [SerializeField] private LeaderboardDataFetcher dataFetcher;
        [SerializeField] private LeaderboardUI leaderboardUI;
        [SerializeField] private LoadingUI loadingUI;
        
        [Space(10)] 
        [Header("UI Elements")]
        [SerializeField] private Button openLeaderboardButton;
        [SerializeField] private Button closeLeaderboardButton;
        [SerializeField] private List<Button> pageButtons;
        
        private void Start()
        {
            dataFetcher.FetchLeaderboardData(0);
        }
        
        #region FetchProcess
        
        private void OnLeaderboardDataFetched(LeaderboardData leaderboardData)
        {
            SetPageButtonStates(true);
            loadingUI.HideUI();

            if(leaderboardData == null)
            {
                return;
            }
            
            // Build leaderboard with fetched data
            leaderboardUI.BuildLeaderboard(leaderboardData);
        }

        private void FetchLeaderboardData(int pageNumber)
        {
            // Start fetching and parsing data and show loading UI in that time
            loadingUI.ShowUI();
            dataFetcher.FetchLeaderboardData(pageNumber);
        }
        #endregion FetchProcess
        
        
        #region ButtonFunctions
        
        private void OnClickCloseLeaderboardButton()
        {
            // Close leaderboard and move return main menu
            AudioManager.instance.PlaySoundFx(SoundType.Button);
            CanvasManager.instance.SwitchCanvasSync(CanvasManager.CanvasType.MainMenu);
        }
        private void OnClickPageButton(int index)
        {
            // Fetch selected page data
            AudioManager.instance.PlaySoundFx(SoundType.Button);
            SetPageButtonStates(false);
            FetchLeaderboardData(index);
        }

        private void SetPageButtonStates(bool state)
        {
            foreach (Button button in pageButtons)
            {
                button.enabled = state;
            }
        }
        #endregion ButtonFunctions
        
        
        
        public override float ShowCanvas()
        {
            transform.localScale = Vector3.zero;

            canvas.enabled = true;

            transform.DOKill();
            transform.position = openLeaderboardButton.transform.position;
            transform.DOLocalMove(Vector3.zero, 0.5f);
            transform.DOScale(Vector3.one, 0.5f).OnComplete(leaderboardUI.ResetScrollPosition);
            
            FetchLeaderboardData(0);
            
            return 0f;
        }

        public override float HideCanvas()
        {
            transform.DOMove(openLeaderboardButton.transform.position, 0.5f);
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => 
            {
                canvas.enabled = false;
            });
            
            return 0.5f;
        }

        public override void HideOnStart()
        {
            canvas.enabled = false;
        }


        private void OnEnable()
        {
            for (int i = 0; i < pageButtons.Count; i++)
            {
                int index = i;
                pageButtons[i].onClick.AddListener(delegate {OnClickPageButton(index);});
            }
            
            closeLeaderboardButton.onClick.AddListener(OnClickCloseLeaderboardButton);
            dataFetcher.OnDataFetched += OnLeaderboardDataFetched;
        }

        private void OnDisable()
        {
            for (int i = 0; i < pageButtons.Count; i++)
            {
                int index = i;
                pageButtons[i].onClick.RemoveListener(delegate {OnClickPageButton(index);});
            }
            
            closeLeaderboardButton.onClick.RemoveListener(OnClickCloseLeaderboardButton);
            dataFetcher.OnDataFetched -= OnLeaderboardDataFetched;
        }
    }
}