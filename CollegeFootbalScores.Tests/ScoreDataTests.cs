using NUnit.Framework;
using System.Threading.Tasks;
using CollegeFootballScores.Common;
using CollegeFootballScores.Model;

namespace CollegeFootbalScores.Tests
{
    // Naming Conventions: 
    // TestCase- MethodName_StateUnderTest
    // Test (individual)- MethodName_StateUnderTest_ExpectedBehavior

    [TestFixture]
    public class ScoreDataTests
    {

        //[TestCase(LeagueCode.Sec , 2017, 9)]
        //[TestCase(LeagueCode.Sec, 2017, 4)]
        //[TestCase(LeagueCode.Big12, 2017, 5)]
        ////[TestCase("BIG12", 2017, 6)]
        ////[TestCase("top25", 2017, 9)]
        ////[TestCase("PAC12", 2017, 9)]
        ////[TestCase("BIG10", 2017, 5)]
        ////[TestCase("ACC", 2017, 5)]
        ////[TestCase("IA", 2017, 5)]  //FBS Independents
        //public async Task GetGamesFromPage_GetGames(LeagueCode league, int year, int week)
        //{
        ////    // Arrange
        ////    var scoreData = new Repository();

        ////    // Act
        ////    var gamesFromPage = await scoreData.GetGamesByLeagueAndWeek(league, year, week);


        ////    // Assert
        ////    Assert.AreEqual(59, gamesFromPage.Count);
        ////}

    }
}
