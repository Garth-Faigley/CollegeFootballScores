using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CollegeFootballScores.Extensions;
using CollegeFootballScores.Model;
using HtmlAgilityPack;

namespace CollegeFootballScores.Common
{
    public static class Repository
    {
        private const string UrlRoot = "https://www.cbssports.com/college-football/scoreboard/";
        private static readonly HtmlWeb Web = new HtmlWeb();

        public static async Task<List<Game>> GetGamesByLeagueAndWeek(string league, int year, int week)
        {
            // return DummmyDevGameData();

            var games = new List<Game>();
            var url = UrlRoot + league + "/" + year + "/regular/" + week + "/";
            var doc = await Task.Factory.StartNew(() => Web.Load(url));

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
                    // Status = statusNode.InnerText.Trim().Replace(" ", "\r\n"),
                    Status = ProcessStatus(statusNode),
                    AwayTeam = ProcessTeam(awayTeamNode.SelectSingleNode(".//a[@class = 'team']").InnerText.Trim()),
                    AwayRank = (awayRankNodes.Count == 0) ? string.Empty : awayRankNodes.First().InnerText.Trim(),
                    AwayScore = (scoreNodes.Count == 0) ? string.Empty : scoreNodes[0].InnerText.Trim(),
                    HomeTeam = ProcessTeam(homeTeamNode.SelectSingleNode(".//a[@class = 'team']").InnerText.Trim()),
                    HomeRank = (homeRankNodes.Count == 0) ? string.Empty : homeRankNodes.First().InnerText.Trim(),
                    HomeScore = (scoreNodes.Count == 0) ? string.Empty : scoreNodes[1].InnerText.Trim()
                };

                games.Add(newGame);
            }

            return games;
        }

        private static string ProcessTeam(string rawTeam)
        {
            var processTeam = rawTeam.Replace("&amp;", "&");
            processTeam = processTeam.Replace("State", "St.");
            return processTeam;
        }

        private static string ProcessStatus(HtmlNode statusNode)
        {
            var status = string.Empty;
            var doesContainTime = false;

            var statusArray = statusNode.InnerText.Trim().Split(' ');
            foreach (var statusPart in statusArray)
            {
                var statusTime = new DateTime();
                if (DateTime.TryParseExact(statusPart, "h:mmtt", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal, out statusTime))
                {
                    doesContainTime = true;
                    status = statusTime.AddHours(-1).ToShortTimeString();
                }
                if (!doesContainTime)
                {
                    status = statusNode.InnerText.Trim();
                    status = status.Replace(" ", "\r\n");
                    status = status.Replace("HALFTIME", "HALF");
                    status = status.Replace("/", "\r\n");
                }
            }

            return status;
        }



        private static List<Game> DummmyDevGameData()
        {
            var dummyGames = new List<Game>();

            dummyGames.Add(new Game()
            {
                AwayTeam = "Peoria",
                AwayScore = "17",
                HomeTeam = "Dallas",
                HomeScore = "7",
                Status = "2ND 10:06".Replace(" ", "\r\n")
            });

            dummyGames.Add(new Game()
            {
                AwayTeam = "Da Bears",
                AwayRank = "22",
                HomeTeam = "Da Saints",
                Status = "THU 6:30pm".Replace(" ", "\r\n")
            });

            dummyGames.Add(new Game()
            {
                AwayRank = "13",
                AwayScore = "6",
                AwayTeam = "Vanderbilt",
                HomeRank = "1",
                HomeScore = "22",
                HomeTeam = "USC",
                Status = "Final".Replace(" ", "\r\n")
            });


            return dummyGames;
        }

    }
}
