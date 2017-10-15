using System.Collections.Generic;
using System.Threading.Tasks;
using CollegeFootballScores.Model;

namespace CollegeFootbalScores.Data
{
    public interface IScoreData
    {
        Task<List<Game>> GetGamesByLeagueAndWeek(LeagueCode league, int year, int week);
    }
}
