using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CollegeFootballScores.Common;
using CollegeFootballScores.Model;

namespace CollegeFootballScores
{
    public class MainWindowViewModel : ObservableObject
    {
        private List<Game> _games;
        private readonly List<KeyValuePair<string, string>> _leagues;
        public ICommand StartTickerCommand { get; set; }

        public MainWindowViewModel()
        {
            StartTickerCommand = new RelayCommand(OnStartTickerCommand);
            _leagues = LoadLeagues();
            SetDefaultValues();
        }

        private void OnStartTickerCommand()
        {

            Task.Run((Action)RunScoreTicker);
        }

        private void SetDefaultValues()
        {
            SelectedLeague = "ORGANIZATION NAME HERE";
            SelectedGame = new Game()
            {
                AwayTeam = "EVENT NAME HERE",
                HomeTeam = "SportsCenter"
            };
        }

        // TODO- Add way for users to enter the year, week, and choose leagues
        private async void RunScoreTicker()
        {
            while (true)
            {
                foreach (var league in _leagues)
                {
                    var games = await Repository.GetGamesByLeagueAndWeek(league.Key, 2017, 9);
                    SelectedLeague = league.Value;

                    foreach (var game in games)
                    {
                        SelectedGame = game;
                        Thread.Sleep(4000);
                    }

                    SetDefaultValues();
                    Thread.Sleep(3000);
                }
            }
        }

        private Game _selectedGame;

        public Game SelectedGame
        {
            get { return _selectedGame; }
            set { SetProperty(ref _selectedGame, value); }
        }

        private string _selectedLeague;

        public string SelectedLeague
        {
            get { return _selectedLeague; }
            set { SetProperty(ref _selectedLeague, value); }
        }


        // The ticker only displays the top 25, power 5 conferences, and major independents (Notre Dame, BYU, etc.)
        // Other leagues can be added here if desired.
        private List<KeyValuePair<string, string>> LoadLeagues()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("top25", "TOP 25"),
                new KeyValuePair<string, string>("BIG12", "BIG 12"),
                new KeyValuePair<string, string>("Sec", "SEC"),
                new KeyValuePair<string, string>("PAC12", "PAC 12"),
                new KeyValuePair<string, string>("BIG10", "BIG 10"),
                new KeyValuePair<string, string>("ACC", "ACC"),
                new KeyValuePair<string, string>("IA", "Independents")
            };
        }

    }
}
