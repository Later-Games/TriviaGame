using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace TriviaGame.UI.Leaderboard
{
    public class LeaderboardDataFetcher : MonoBehaviour
    {
        [SerializeField] private Settings settings;
        
        public event Action<LeaderboardData> OnDataFetched;
        
        private Coroutine _currentCoroutine;
        
        public void FetchLeaderboardData(int pageNumber)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = StartCoroutine(GetLeaderboardDataForPage(pageNumber));
        }

        IEnumerator GetLeaderboardDataForPage(int pageNumber)
        {
            // Create a UnityWebRequest to fetch data from the API
            using (UnityWebRequest request = UnityWebRequest.Get(GetUrlForPage(pageNumber)))
            {
                // Send the request and wait for a response
                yield return request.SendWebRequest();

                // Check for errors
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error fetching leaderboard data: " + request.error);
                    OnDataFetched?.Invoke(null);
                }
                else
                {
                    // Parse and process the response
                    string jsonResponse = request.downloadHandler.text;

                    try
                    {
                        LeaderboardData leaderboard = JsonConvert.DeserializeObject<LeaderboardData>(jsonResponse);
                        OnDataFetched?.Invoke(leaderboard);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        OnDataFetched?.Invoke(null);
                    }
                }
            }
        }

        // adjust url for page number
        private string GetUrlForPage(int pageNumber)
        {
            string pageName = $"leaderboard_page_{pageNumber}.json";

            return settings.dataApiUrlBase + pageName;
        }
    }

}