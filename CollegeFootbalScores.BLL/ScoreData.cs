using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFootballScores.Model;
using CollegeFootbalScores.Data.Extensions;

namespace CollegeFootbalScores.Data 
{
    public class ScoreData : IScoreData
    {
        private const string UrlRoot = "https://www.cbssports.com/college-football/scoreboard/";
        readonly HtmlWeb _web = new HtmlWeb();

        public async Task<List<Game>> GetGamesByLeagueAndWeek(LeagueCode league, int year, int week)
        {
            return DummmyDevGameData();

            var games = new List<Game>();
            var url = UrlRoot + league + "/" + year + "/regular/" + week + "/";
            var doc = await Task.Factory.StartNew(() => _web.Load(url));

            var gameNodes = doc.DocumentNode.SelectNodes("//div[@id[starts-with(., 'game-') and string-length() > 5]]");
            if (gameNodes == null) return games;

            foreach (var gameNode in gameNodes)
            {
                var statusNode = gameNode.SelectNodes(".//div[@class[starts-with(., 'game-status')]]").First();

                var awayTeamNode = gameNode.SelectNodes(".//td[@class = 'team']")[0];
                var homeTeamNode = gameNode.SelectNodes(".//td[@class = 'team']")[1];

                var scoreNodes = gameNode.SelectNodes(".//td[@class = 'total-score']").NullGard();

                var awayRankNodes = awayTeamNode.SelectNodes(".//span[@class = 'rank']").NullGard();
                var homeRankNodes = homeTeamNode.SelectNodes(".//span[@class = 'rank']").NullGard();

                var newGame = new Game
                {
                    Status = statusNode.InnerText.Trim(),
                    AwayTeam = awayTeamNode.SelectSingleNode(".//a[@class = 'team']").InnerText.Trim(), 
                    AwayRank = (awayRankNodes.Count == 0) ? string.Empty : awayRankNodes.First().InnerText.Trim(), 
                    AwayScore = (scoreNodes.Count == 0) ? string.Empty : scoreNodes[0].InnerText.Trim(), 
                    HomeTeam = homeTeamNode.SelectSingleNode(".//a[@class = 'team']").InnerText.Trim(),
                    HomeRank = (homeRankNodes.Count == 0) ? string.Empty : homeRankNodes.First().InnerText.Trim(), 
                    HomeScore = (scoreNodes.Count == 0) ? string.Empty : scoreNodes[1].InnerText.Trim()
                };

                games.Add(newGame);
            }

            return games;
        }

        private List<Game> DummmyDevGameData()
        {
            var dummyGames = new List<Game>();

            dummyGames.Add(new Game()
            {
                AwayScore = "6", 
                AwayTeam = "Doss Owls", 
                HomeRank = "1", 
                HomeScore = "22", 
                HomeTeam = "Hill Dillos", 
                Status = "Final"
            });

            dummyGames.Add(new Game()
            {
                AwayTeam = "Da Bears",
                AwayRank = "22",
                HomeTeam = "Da Saints",
                Status = "THU 6:30pm"
            });

            dummyGames.Add(new Game()
            {
                AwayTeam = "Murchison",
                AwayScore = "17",
                HomeTeam = "Dallas",
                HomeScore = "7",
                Status = "2ND 10:06"
            });

            return dummyGames;
        }
       
    }
}
