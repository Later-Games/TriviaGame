
namespace TriviaGame.UI.Leaderboard
{
    public class LeaderboardData
    {
        public int page;
        public bool isLast;
        public LeaderboardUserData[] data;
    }
    
    public class LeaderboardUserData
    {
        public string nickname;
        public int rank;
        public int score;
    }

}